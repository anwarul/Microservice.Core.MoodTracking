using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RS.MF.MoodTracking.Application.Dtos;
using RS.MF.MoodTracking.Application.ProjectionModel;
using RS.MF.MoodTracking.Application.Queries;
using RS.MF.MoodTracking.Application.QueryDtos;
using System.Linq.Expressions;

namespace RS.MF.MoodTracking.Infrastructure.Services;

public class MoodTrackingQueryService : IMoodTrackingQueryService
{
    private readonly IMoodTrackingRepository _repository;
    private readonly ISecurityContextProvider _contextProvider;

    public MoodTrackingQueryService(
        IMoodTrackingRepository moodTracking,
        ISecurityContextProvider securityContext
        )
    {
        _repository = moodTracking;
        _contextProvider = securityContext;
    }

    public async ValueTask<PagedResult<MoodTrackingLogView>> GetLogViewAsync(CommonQueryDto pagedQuery, CancellationToken cancellationToken)
    {

        Expression<Func<MoodTrackingLog, MoodTrackingLogView>> projection = obj => new MoodTrackingLogView
        {
            ItemId = obj.ItemId,
            ActionDateTime = obj.ActionDateTime,
            Activity = obj.Activity,
            Mood = obj.Mood,
            Note = obj.Note,

        };
        var query = await _repository.Query(_contextProvider.GetSecurityContext().TenantId, x => true, projection);


        if (pagedQuery.StartDate != null && pagedQuery.EndDate != null)
        {
            var endDate = pagedQuery.StartDate == pagedQuery.EndDate ? pagedQuery.StartDate.Value.AddHours(23).AddMinutes(59) : pagedQuery.EndDate;
            query = query.Where(obj => obj.CreateDate >= pagedQuery.StartDate && obj.CreateDate <= endDate);
        }
        else if (pagedQuery.StartDate != null && pagedQuery.EndDate is null)
        {
            var startDate = pagedQuery.StartDate;
            var endDate = pagedQuery.StartDate.Value.AddDays(1);
            query = query.Where(obj => obj.CreateDate >= startDate && obj.CreateDate < endDate);
        }
        if (pagedQuery.UserId != null)
        {
            query = query.Where(obj => obj.CreatedByUserId == pagedQuery.UserId);
        }

        var data = await DataPagination.PaginateWithTotalAsync(query, pagedQuery);
        return PagedResult<MoodTrackingLogView>.Create(data.Items, data.CurrentPage, data.ResultsPerPage, data.TotalPages, data.TotalResults);

    }
    public async ValueTask<IEnumerable<MoodSummary>> GetMoodSummaries(UserMoodSummaryQuery pagedQuery, CancellationToken cancellationToken)
    {
        var response = new List<UserMoodSummary>();
        var query = await _repository.Query(_contextProvider.GetSecurityContext().TenantId);

        if (pagedQuery.StartDate != null && pagedQuery.EndDate != null)
        {
            var startDate = new DateTime(pagedQuery.StartDate.Value.Year, pagedQuery.StartDate.Value.Month, pagedQuery.StartDate.Value.Day, 0, 0, 0, DateTimeKind.Utc);
            var endDate = new DateTime(pagedQuery.EndDate.Value.Year, pagedQuery.EndDate.Value.Month, pagedQuery.EndDate.Value.Day, 0, 0, 0, DateTimeKind.Utc);
            endDate = endDate.AddHours(23).AddMinutes(59).AddSeconds(59);

            query = query.Where(obj => obj.CreateDate >= startDate && obj.CreateDate <= endDate);
        }
        else if (pagedQuery.StartDate != null && pagedQuery.EndDate is null)
        {
            var startDate = new DateTime(pagedQuery.StartDate.Value.Year, pagedQuery.StartDate.Value.Month, pagedQuery.StartDate.Value.Day, 0, 0, 0, DateTimeKind.Utc);
            var endDate = startDate.AddHours(23).AddMinutes(59).AddSeconds(59);

            query = query.Where(obj => obj.CreateDate >= startDate && obj.CreateDate <= endDate);
        }

        if (pagedQuery.UserId != null)
        {
            query = query.Where(obj => obj.CreatedByUserId == pagedQuery.UserId);
        }


        return query.GroupBy(x => x.Mood).Select(x => new MoodSummary
        {
            Mood = x.Key,
            Count = x.Count(),
        });
    }
    public async ValueTask<IEnumerable<MoodTrackingView>> GetMoodTrackings(UserMoodQuery pagedQuery, CancellationToken cancellationToken)
    {
        var query = await _repository.Query(_contextProvider.GetSecurityContext().TenantId);

        if (pagedQuery.StartDate != null && pagedQuery.EndDate != null)
        {
            var endDate = pagedQuery.StartDate == pagedQuery.EndDate ? pagedQuery.StartDate.Value.AddHours(23).AddMinutes(59) : pagedQuery.EndDate;
            query = query.Where(obj => obj.CreateDate >= pagedQuery.StartDate && obj.CreateDate <= endDate);
        }
        else if (pagedQuery.StartDate != null && pagedQuery.EndDate is null)
        {
            var startDate = pagedQuery.StartDate;
            var endDate = pagedQuery.StartDate.Value.AddDays(1);
            query = query.Where(obj => obj.CreateDate >= startDate && obj.CreateDate < endDate);
        }
        else if (pagedQuery.Dates != null)
        {
            var dateString = pagedQuery.Dates.Split(",");
            var dateList = new List<DateTime>();
            try
            {
                foreach (var date in dateString)
                {
                    var dateTime = DateTime.Parse(date);
                    var dat = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0, DateTimeKind.Utc);

                    dateList.Add(dat);
                }
                query = query.Where(obj => dateList.Contains(obj.CreateDate.Date));
            }
            catch (Exception e)
            {
                throw new InvalidCastException("Date conversion failed ! Please set valid date format.");
            }
        }

        if (pagedQuery.UserId != null)
        {
            query = query.Where(obj => obj.CreatedByUserId == pagedQuery.UserId);
        }

        return query.GroupBy(x => x.CreateDate.Date).Select(x => new
        {
            date = x.Key,
            mood = x.Select(obj => new MoodTrackingLogBasic
            {
                ActionDateTime = obj.ActionDateTime,
                Activity = obj.Activity,
                Mood = obj.Mood,
                Note = obj.Note,
            })
        }).AsEnumerable().Select(x => new MoodTrackingView
        {
            Date = x.date,
            UserMood = new UserMoodTrackView
            {
                MoodCount = x.mood.GroupBy(v => new { v.Mood.ReferenceItemId, v.Mood.Name, v.Mood.ImageUrl }).Select(m => new MoodSummary { Mood = new MoodEntryReference { ReferenceItemId = m.Key.ReferenceItemId, Name = m.Key.Name, ImageUrl = m.Key.ImageUrl }, Count = m.Count() }),
                MoodTrackings = x.mood.ToList(),
            }
        });
    }
    public async ValueTask<MoodAndActivitySummariesView> GetDatewiseMoodAndActivitySummaries(MoodAndActivitySummariesQuery pagedQuery, CancellationToken cancellationToken)
    {
        try
        {

            MoodAndActivitySummariesView response = new MoodAndActivitySummariesView();
            var query = await _repository.Query(_contextProvider.GetSecurityContext().TenantId);

            if (pagedQuery.StartDate != null && pagedQuery.EndDate != null)
            {
                query = query.Where(obj => obj.CreateDate >= pagedQuery.StartDate && obj.CreateDate <= pagedQuery.EndDate);
            }
            if (pagedQuery.StartDate != null)
            {
                query = query.Where(obj => obj.CreateDate.Date == pagedQuery.StartDate);
            }
            if (pagedQuery.UserId != null)
            {
                query = query.Where(obj => obj.CreatedByUserId == pagedQuery.UserId);
            }

            var items = query.ToList();

            var accepts = pagedQuery.Accepts.Split(",");

            if (accepts.Contains("mood"))
            {

                var dateGroupingData = items.GroupBy(x => x.CreateDate.Date).Select(x => new
                {
                    date = x.Key,
                    records = x.ToList()
                }).ToList();


                List<UserMoodSummary> moodSummaries = new List<UserMoodSummary>();
                dateGroupingData.ForEach(obj =>
                {

                    var data = obj.records.GroupBy(x => new { x.Mood?.Name, x.Mood?.ImageUrl, x.Mood?.ReferenceItemId }).Select(item => new MoodSummary
                    {
                        Mood = new MoodEntryReference { Name = item?.Key?.Name, ImageUrl = item?.Key?.ImageUrl, ReferenceItemId = item.Key.ReferenceItemId ?? Guid.Empty },
                        Count = item.Count()
                    }).ToList();
                    var record = new UserMoodSummary
                    {
                        Date = obj.date,
                        MoodSummaries = data
                    };
                    moodSummaries.Add(record);
                });

                response.MoodSummaries = moodSummaries;
            }

            if (accepts.Contains("avgmood"))
            {

                var dateGroupingData = items.GroupBy(x => x.CreateDate.Date).Select(x => new
                {
                    date = x.Key,
                    records = x.ToList()
                }).ToList();


                //List<UserMoodSummary> moodSummaries = new List<UserMoodSummary>();
                //dateGroupingData.ForEach(obj =>
                //{

                //    var data = obj.records.GroupBy(x => new { x.Mood?.Name, x.Mood?.ColorCode, x.Mood?.SvgImage, x.Mood?.ReferenceItemId }).Select(item => new MoodSummary
                //    {
                //        Mood = new MoodEntryView { Name = item?.Key?.Name, ColorCode = item?.Key?.ColorCode, SvgImage = item?.Key?.SvgImage, ReferenceItemId = item.Key.ReferenceItemId ?? Guid.Empty },
                //        Count = item.Count()
                //    }).ToList();
                //    var record = new MoodSummary
                //    {
                //        Date = obj.date,
                //        MoodSummaries = data
                //    };
                //    moodSummaries.Add(record);
                //});

                response.AverageMoodSummaries = null;
            }

            if (accepts.Contains("activity"))
            {
                List<MoodActivityReference> activities = new List<MoodActivityReference>();
                items.ForEach(item =>
                {
                    activities.AddRange(item.Activity);
                });


                response.TopActivities = activities.GroupBy(x => new { x?.Name, x?.ImageUrl, x?.ReferenceItemId }).Select(item => new ActivitySummary
                {
                    Activity = new MoodActivityReference { Name = item?.Key?.Name, ImageUrl = item?.Key?.ImageUrl, ReferenceItemId = item.Key.ReferenceItemId ?? Guid.Empty },
                    Count = item.Count()
                }).OrderByDescending(x => x.Count).Take(5).ToList();
            }

            if (accepts.Contains("moodcount"))
            {
                response.MoodCount = items.GroupBy(x => new { x.Mood?.Name, x.Mood?.ImageUrl, x.Mood?.ReferenceItemId }).Select(item => new MoodSummary
                {
                    Mood = new MoodEntryReference { Name = item?.Key?.Name, ImageUrl = item?.Key?.ImageUrl, ReferenceItemId = item.Key.ReferenceItemId ?? Guid.Empty },
                    Count = item.Count()
                }).ToList();
            }


            return response;
        }
        catch (Exception e)
        {
            throw e;
        }
    }
    public async ValueTask<IEnumerable<YearlyMoodSummaryView>> GetYearlyMoodSummaries(YearlyMoodSummaryQuery pagedQuery, CancellationToken cancellationToken)
    {


        var query = await _repository.Query(_contextProvider.GetSecurityContext().TenantId);

        var data = query.Where(x => x.ActionDateTime.Year == pagedQuery.Year)
            .GroupBy(x => new { x.ActionDateTime.Date, x.Mood.ReferenceItemId })
            .Select(x => new
            {
                Date = x.Key.Date,
                MoodId = x.Key.ReferenceItemId,
                MoodCount = x.Count(),
            })
            .OrderByDescending(x => x.MoodCount)
            .GroupBy(x => x.Date).Select(x => x.First()).ToList();

        var monthlySummary = new List<YearlyMoodSummaryView>();


        for (var month = 1; month <= 12; month++)
        {
            monthlySummary.Add(new YearlyMoodSummaryView
            {
                Month = month,
                DailyMoods = data.Where(x => x.Date.Month == month).Select(x => new MonthlyMoodSummaryView
                {
                    MoodId = x.MoodId,
                    Day = x.Date.Day,
                }).ToList()
            });
        }

        return monthlySummary;
    }
}
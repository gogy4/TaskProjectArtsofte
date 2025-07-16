using System.Text;
using Application.Models;
using Shared.Services;

namespace Application.Services.Helpers.Implementations;

public class JobHelper : IJobHelper
{
    public async Task<JobDto> GetCreatedByCurrentUserAsync(JobDto jobDto, HttpClient httpClient)
    {
        var user = await UserHelper.GetCurrentUserId(httpClient);
        if (jobDto.CreatedUserId != user.Id)
        {
            throw new ArgumentException("У вас не достаточно прав.");
        }

        return jobDto;
    }

    public async Task<JobDto> GetCreatedOrAssignedByUserAsync(JobDto jobDto, HttpClient httpClient)
    {
        var user = await UserHelper.GetCurrentUserId(httpClient);
        if (jobDto.CreatedUserId != user.Id && jobDto.AssignedUserId != user.Id)
        {
            throw new ArgumentException("Эта задача вам не доступна.");
        }

        return jobDto;
    }

    public string GetDifferenceField(JobDto oldJob, JobDto newJob)
    {
        var builder = new StringBuilder();
        var oldType = oldJob.GetType();
        var newType = newJob.GetType();

        foreach (var prop in newType.GetProperties())
        {
            var oldProp = oldType.GetProperty(prop.Name);
            if (oldProp == null) continue;
            var oldValue = oldProp.GetValue(oldJob);
            var newValue = prop.GetValue(newJob);
            if (!Equals(oldValue, newValue))
            {
                builder.Append($"{prop.Name} изменилось с '{oldValue}' на '{newValue}'; ");
            }
        }

        return builder.ToString();
    }
}
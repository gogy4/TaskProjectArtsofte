using Application.Models;

namespace Application.Services.Helpers;

public interface IJobHelper
{
    Task<JobDto> GetCreatedByCurrentUserAsync(JobDto jobDto, HttpClient httpClient);
    string GetDifferenceField(JobDto oldJob, JobDto newJob);
    Task<JobDto> GetCreatedOrAssignedByUserAsync(JobDto jobDto, HttpClient httpClient);
}
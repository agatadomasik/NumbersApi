using NumbersApi.Models;

namespace NumbersApi.Services;

public interface INumbersService
{
    void AddNumbers(IEnumerable<int> numbers);
    List<int> GetAllNumbers();
    List<int> GetSortedNumbers(string sortOrder);
    SearchResponse SearchNumber(int value);
    StatsResponse GetStatistics();
    Task<ParallelProcessResponse> ProcessNumbersParallelAsync();

}
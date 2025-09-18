using NumbersApi.Models;
using NumbersApi.Services;
using System.Diagnostics;

namespace NumbersApi.Services;

public class NumbersService : INumbersService
{
    private readonly List<int> _numbers = new();

    public void AddNumbers(IEnumerable<int> numbers)
    {
        _numbers.AddRange(numbers);
    }

    public List<int> GetAllNumbers()
    {
        return new List<int>(_numbers);
    }

    public List<int> GetSortedNumbers(string sortOrder)
    {
        var copy = new List<int>(_numbers);

        switch (sortOrder)
        {
            case "desc":
                return QuickSort(copy, true);

            case "asc":
                return QuickSort(copy, false);

            default:
                return QuickSort(copy, false);
        }
    }


    public SearchResponse SearchNumber(int value)
    {
        return new SearchResponse{
            Value = value,
            Found = BinarySearch(_numbers, value)
        };
    }

    public StatsResponse GetStatistics()
    {
        if (_numbers.Count == 0)
        {
            return new StatsResponse
            {
                Average = 0,
                Median = 0
            };
        }

        var sortedNumbers = QuickSort(new List<int>(_numbers), false);

        var average = _numbers.Average();
        var median = Median(sortedNumbers);

        return new StatsResponse
        {
            Average = Math.Round(average, 2),
            Median = median
        };
    }

    public async Task<ParallelProcessResponse> ProcessNumbersParallelAsync()
    {
        int cores = Environment.ProcessorCount;
        int partSize = Math.Max(1, _numbers.Count / cores);

        var tasks = new List<Task<long>>();

        for (int i = 0; i < _numbers.Count; i += partSize)
        {
            int start = i;
            int end = Math.Min(i + partSize, _numbers.Count);

            tasks.Add(Task.Run(() =>
            {
                long localSum = 0;
                for (int j = start; j < end; j++)
                {
                    localSum += _numbers[j];
                }
                return localSum;
            }));
        }

        var results = await Task.WhenAll(tasks);
        var sum = results.Sum();

        return new ParallelProcessResponse
        {
            Count = _numbers.Count,
            Sum = sum,
            Average = _numbers.Count > 0 ? (double)sum / _numbers.Count : 0,
        };
    }

    private List<int> QuickSort(List<int> numbers, bool descending = false)
    {
        if (numbers.Count <= 1) return numbers;

        var pivot = numbers[numbers.Count / 2];
        var left = new List<int>();
        var middle = new List<int>();
        var right = new List<int>();

        foreach (var number in numbers)
        {
            if (number < pivot) left.Add(number);
            else if (number > pivot) right.Add(number);
            else middle.Add(number);
        }

        var sortedLeft = QuickSort(left, descending);
        var sortedRight = QuickSort(right, descending);

        return descending
            ? sortedRight.Concat(middle).Concat(sortedLeft).ToList()
            : sortedLeft.Concat(middle).Concat(sortedRight).ToList();
    }

    private bool BinarySearch(List<int> numbers, int target)
    {
        if (numbers.Count == 0) return false;

        var sorted = QuickSort(new List<int>(numbers), false);
        int left = 0, right = sorted.Count - 1;

        while (left <= right)
        {
            int mid = left + (right - left) / 2;

            if (sorted[mid] == target) return true;
            if (sorted[mid] < target) left = mid + 1;
            else right = mid - 1;
        }

        return false;
    }

    private double Median(List<int> sortedNumbers)
    {
        int count = sortedNumbers.Count;

        if (count % 2 == 0)
        {
            return (sortedNumbers[count / 2 - 1] + sortedNumbers[count / 2]) / 2.0;
        }
        else
        {
            return sortedNumbers[count / 2];
        }
    }
}
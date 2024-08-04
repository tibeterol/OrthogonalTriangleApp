// Tibet Erol
using OrthogonalTriangleApp.Enums;
using OrthogonalTriangleApp.Models;

solveTheAlgorithmAndPrintResults("Triangle",TriangleType.Regular);
Console.WriteLine("\n");
solveTheAlgorithmAndPrintResults("Triangle2",TriangleType.Orthogonal);

Console.ReadKey();


void solveTheAlgorithmAndPrintResults(string fileName = "Triangle2", TriangleType triangleType = TriangleType.Orthogonal)
{
    string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
    string projectRoot = Path.Combine(baseDirectory, @"..\..\..\..\OrthogonalTriangleApp");
    string fullPath = Path.Combine(projectRoot, $"Files\\{fileName}.txt");
    fullPath = Path.GetFullPath(fullPath);

    string[] txtLines = File.ReadAllLines(fullPath);

    List<PathsModel> allPaths = new List<PathsModel>();

    foreach (string line in txtLines)
    {
        var numsInTheLine = line.Trim().Split(" ").Select((num, index) => new LineModel
        {
            value = short.Parse(num),
            index = (short)index
        }).ToList();

        createPaths(allPaths, numsInTheLine, triangleType);
    }

    calculateSumsOfCreatedPaths(allPaths);

    var neededPathsElement = allPaths.OrderByDescending(u => u.sum).First();

    Console.WriteLine("Chosen Path: " + string.Join(", ", neededPathsElement.Path.Select(u => u.value)));
    Console.WriteLine("Sum Value: " + neededPathsElement.sum);
}

bool isThisAprimeNumber(short num)
{
    if (num < 2)
        return false;

    for (short i = 2; i < num; i++)
        if (num % i == 0)
            return false;

    return true;
}

void createPaths(List<PathsModel> paths, List<LineModel> numsInTheLine, TriangleType triangleType = TriangleType.Orthogonal)
{

    if (numsInTheLine.Count == 1)
    {
        if (isThisAprimeNumber(numsInTheLine[0].value))
            throw new Exception("The top element of the triangle must not be a prime number!");

        List<LineModel> newPath = [numsInTheLine[0]];
        paths.Add(new PathsModel { Path = newPath, sum = 0});
    }
    else
    {
            var newPaths = paths.ToList();
            newPaths.ForEach(p =>
            {
                List<LineModel>? adjacentElements;

                if (triangleType == TriangleType.Orthogonal)
                    adjacentElements = p.Path.Last().index == 0 ? numsInTheLine.Where(u => u.index == 0 || u.index == 1).ToList() : numsInTheLine.Where(u => u.index == p.Path.Last().index - 1 || u.index == p.Path.Last().index + 1 || u.index == p.Path.Last().index).ToList();
                else
                    adjacentElements = numsInTheLine.Where(u => u.index == p.Path.Last().index + 1 || u.index == p.Path.Last().index).ToList();

                var adjacentNonPrimeElements = adjacentElements.Where(u => !isThisAprimeNumber(u.value)).ToList();

                if (adjacentNonPrimeElements.Count == 1 && p.stop != true)
                {
                    p.Path.Add(adjacentNonPrimeElements[0]);
                    return;
                }

                else if (adjacentNonPrimeElements.Count > 1 && p.stop != true)
                {
                    var basePath = p.Path.ToList();

                    var maxAdjacentNonPrimeElement = adjacentNonPrimeElements.OrderByDescending(x => x.value).First();
                    p.Path.Add(maxAdjacentNonPrimeElement);

                    adjacentNonPrimeElements.Where(a => a != maxAdjacentNonPrimeElement).ToList().ForEach(x =>
                    {
                        var newPath = basePath.ToList();
                        newPath.Add(x);
                        paths.Add(new PathsModel { Path = newPath, sum = 0 });
                    });
                    return;
                }
                else
                {
                    p.stop = true;
                    return;
                }
            });
    }
}

void calculateSumsOfCreatedPaths(List<PathsModel> paths)
{
    paths.ForEach(p =>
    {
        short sum = 0;
        p.Path.ForEach(x =>
        {
            sum += x.value;
        });
        p.sum = sum;
    });
}
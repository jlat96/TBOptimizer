# Quick Start
## Hill Climber

```HillClimber``` implements a versions of the [Hill Climbing Local Search Algorithm](https://en.wikipedia.org/wiki/Hill_climbing).

### To Set Up a Hill Climber

1. Create a state class that can calculate a utility score that extends ```EvaluableState```. Provide your concrete implementation with a method of evaluating the state.

```cs
public class CardTable : EvaluableState<int>
{
  private readonly Card[,] board;
  public CardTable(List<Cards> cards)
  {
    // set up the state
  }
   
  // Inherited by the abstract EvaluableState
  protected override int Evaluate()
  {
    List<ScoreResult> scores = new List<ScoreResult>();
    for (int i = 0; i < Dimension; i++)
    {
        scores.Add(ScoreColumn(i));
        scores.Add(ScoreRow(i));
    }

    return scores.Select(s => s.Score).Aggregate(0, (p, n) => p + ScoreConstants.GetPokerSquaresScore(n));
  }
}
```
2. Create an implementation of ```ISuccessorGenerator``` that will be responsible for creating a neighborhood of successor states from any given state.

```cs
public class BoardSuccessorGenerator : ISuccessorGenerator<CardTable, int>
{
    // Returns the neighborhood of successor states for the input state
    public IEnumerable<CardTable> GetSuccessors(CardTable state)
    {
        List<CardTable> successors = new List<CardTable>();
        for (int i = 0; i < state.Dimension; i++)
        {
            for (int j = 0; j < state.Dimension; j++)
            {
                successors.AddRange(GetSwaps(state, i, j)); // generates neighbors from board combinations
            }
        }

        return successors;
    }

    private List<CardTable> GetSwaps(CardTable state, int row, int col)
    {
        CardTable current = null;
        List<CardTable> swaps = new List<CardTable>();
        for (int sRow = 0; sRow < state.Dimension; sRow++)
        {
            for (int sCol = 0; sCol < state.Dimension; sCol++)
            {
                current = state.Clone();
                current.Swap(row, col, sRow, sCol);
                swaps.Add(current);
            }
        }
        return swaps;
    }
}
```

3. Set up a Hill Climber with ```ClimberConfiguration```

```cs
IHillClimber<CardTable, int> climber = ClimberConfiguration<CardTable, int>()
  .ClimbsInDirection(ClimberDirection.Maximize)
  .GeneratesSuccessorsWith(new BoardSuccessorGenerator())
  .Build();
```

4. Use the Hill Climber

```cs
List<Card> cards = GetInitialStateCards();
CardTable initialTable = new CardTable(cards);
CardTable optimizedTable = climber.Optimize(table);
```


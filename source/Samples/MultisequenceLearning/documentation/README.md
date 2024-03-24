# ML22/23-15   Approve Prediction of Multisequence Learning 

## Introduction

This experiment to understand how multisequence learning algorithm learns sequences and prediction works, implement new mothods to read and learn data from file and then test the samples with sample subsequences to calculate the prediction accuracy.

## Implementation

![image]()

Fig: Overview of the project

`Sequence` is our main data model of the sequences which are learned by the multisequence learning algorithm.

```csharp
public class Sequence
{
    public String name { get; set; }
    public int[] data { get; set; }
}
```

eg: 

- Dataset:

```json
[
  {
    "name": "S1",
    "data": [ 4, 5, 6, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 23, 24, 25, 27, 30, 32, 33, 34 ]
  },
  {
    "name": "S2",
    "data": [ 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 18, 19, 20, 21, 24, 25, 27, 30, 31, 32, 33, 34 ]
  },
  {
    "name": "S3",
    "data": [ 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 17, 18, 19, 21, 22, 23, 24, 25, 26, 27, 28, 30, 31, 33, 34 ]
  }
]
```

- Test Dataset:

```json
[
  {
    "name": "T1",
    "data": [ 24, 25, 27, 28, 29 ]
  },
  {
    "name": "T2",
    "data": [ 13, 15, 16, 17, 18 ]
  },
  {
    "name": "T3",
    "data": [ 19, 21, 22, 23, 24 ]
  },
  {
    "name": "T4",
    "data": [ 17, 18, 19, 20, 21 ]
  }
]
```

Our project is mainly divided into 3 helper methods:

1. [MultisequenceHelper.cs](../MultisequenceHelper.cs)


2. [DatasetHelper.cs](../DatasetHelper.cs)


3. [TestdatasetHelper.cs](../TestdatasetHelper.cs)
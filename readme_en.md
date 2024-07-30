[简体中文](./readme.md) | **English**

---

<h4 align="center">
    <a href=#Features> Features </a> |
    <a href=#Installation> Installation </a> |
    <a href=#Quick-start> Quick Start </a> 
</h4>

## Features

Provide AC automaton, BM25, edit distance, logger, timer and progress show.

## Installation

Right click the project name, and choose the 'Manage Nuget Packages' option, type 'SalaryUtils' in the search box and then install.

## Quick Start

- **ACAutomaton**
  
  ```C#
  using SalaryUtils;

  var ac = new ACAutomaton();
  ac.Build(patterns: ["Software Engineer", "Artificial Intelligence"], ignoreCases: true);
  var res = ac.Search(text: "software engineer and artificial intelligence");
  foreach (var (start, end, text) in res)
      Console.WriteLine($"{start}\t{end}\t{text}");

  //result
  /*
  0       17      software engineer
  22      45      artificial intelligence
  */
  ```

  ```C#
  using SalaryUtils;

  var ac = new ACAutomaton();
  ac.Build(patterns: new string[][] { new string[] { "Software", "Engineer" }, new string[] { "Artificial", "Intelligence" } }, ignoreCases: true);
  var res = ac.Search(text: new string[] { "software", "engineer", "and", "artificial", "intelligence" });
  foreach (var (start, end, text) in res)
      Console.WriteLine($"{start}\t{end}\t{string.Join(" ", text)}");
  
  //result
  /*
  0       2       software engineer
  3       5       artificial intelligence
  */
  ```
- **BM25**
  ```C#
  using SalaryUtils;
  /*to_search.txt
      Software Engineer
      Artificial Intelligence
  */
  var bm25 = new BM25(docs_file: "to_search.txt");
  var res = bm25.Search(query: "engineer", top_k: 5);
  foreach (var (score, text) in res)
      Console.WriteLine($"{score:0.00}\t{text}");

  //result
  /*
  0.51    Software Engineer
  */  
  ```

- **EditDistance**
  ```C#
  using SalaryUtils;
  var distance = EditDistance.GetEditDistance("book", "back");
  Console.WriteLine(distance);

  //result
  /*
  2
  */  
  ```

- **Logger**
  ```C#
  using SalaryUtils;
  Logger.Info("SalaryUtils");

  //result
  /*
  2024-07-30 09:41:02.367 - SalaryUtils
  */ 
  ```

- **Timer**
  
  Similar to the built-in Stopwatch
  ```C#
  using SalaryUtils;
  Timer.Start();
  Thread.Sleep(1000);
  Timer.End();
  Console.WriteLine($"time used: {Timer.ElapsedDuration}");
  Console.WriteLine($"time used: {Timer.ElapsedSeconds} seconds");
  Console.WriteLine($"time used: {Timer.ElapsedMilliseconds} milliseconds");

  //result
  /*
  time used: 0:00:01
  time used: 1.011633544921875 seconds
  time used: 1011.633544921875 milliseconds
  */ 
  ```
  ```C#
  using System.Diagnostics;

  var stopwatch = new Stopwatch();
  stopwatch.Start();
  Thread.Sleep(1000);
  stopwatch.Stop();
  Console.WriteLine($"time used: {stopwatch.Elapsed}");
  Console.WriteLine($"time used: {stopwatch.ElapsedMilliseconds} milliseconds");

  //result
  /*
  time used: 00:00:01.0109459
  time used: 1010 milliseconds
  */ 
  ```

- **ProgressShow**
  ```C#
  using SalaryUtils;

  foreach (var i in Enumerable.Range(0, 10))
      ProgressShow.Show(total: 10, step: 2, desc: "progress show");
  ProgressShow.LastShow();

  //result
  /*
  progress show: 10 | 100.00% | 10
  */ 
  ```
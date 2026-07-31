# Visual regression baselines

This folder contains the committed baseline PNG images used by the
`ExampleSnapshotTests` (`Category=Visual`) in this project.

## How it works

- Each example window is rendered off-screen at 1024x768 / 96 DPI and compared
  against `<ExampleTypeFullName>.png` in this folder.
- If no baseline exists, the test is reported as *inconclusive* and a candidate
  image is written to the test output directory (`TestResults`/work directory
  `Snapshots` folder, also attached to the test result).
- If a baseline exists, the images are compared with a per-channel tolerance of
  10/255 and the test fails when more than 0.5 % of the pixels differ.

## Updating baselines

1. Run the visual tests on a Windows machine or download the snapshot artifacts
   from the CI run:
   `dotnet test Source/PropertyTools.Wpf.ExampleTests --filter TestCategory=Visual`
2. Review the candidate images in the test output.
3. Copy the approved images into this folder and commit them.

Baselines are rendering-environment sensitive (fonts, DPI). Generate them on the
same OS image as CI (`windows-latest`) to keep them stable.

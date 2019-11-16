# Facility Layout Visualization
This project uses Windows Presentation Foundation (WPF) to visualize the actual move when CMA-ES optimizes the facility layout problem.

The following animate is the appearance of the interface.
![image](https://github.com/CodeGoood/Facility_layout/blob/master/CMAES_GUI.gif)

## Arguments Setting
#### There are some arguments you need to input before starting the opitmization.
**Popluation size** denotes the number of indivisual in CMA-ES.  
**Generation** denotes the critiria of this optimization.  
**Alpha, Beta, and Gamma** denote the parameters in CMA-ES.  

## Anytime Behavior
This part will present the generation and the corresponding fitness in a real-time chart.

## Canvas
#### The right canvas shows the facility layout at every generation
The facility layout is randomly generated in the beginning. When the algorithm stop, you can adjust the position of each block by moving the block. When the block you're moving aligns to any else block, all the aligned blocks will change to red.  
Following animate presents how it works.  

![image](https://github.com/CodeGoood/Facility_layout/blob/master/Block%20move.gif)

## Output
#### The below gray box outputs the current state explanation

# Facility_layout
This project uses Windows Presentation Foundation (WPF) to visualize the actual move when CMA-ES optimizes the facility layout problem.

The following picture is the appearance of the interface.
![image](https://github.com/CodeGoood/Facility_layout/blob/master/CMAES_GUI.gif)

## Arguments Setting
#### There are some arguments you need to input before press the start button.  
**Popluation size** denotes the number of indivisual in CMA-ES.  
**Generation** denotes the critiria of this optimization.  
**Alpha, Beta, and Gamma** denote the parameters in CMA-ES.  

## Anytime Behavior
This part will present the generation and the corresponding fitness in a real-time chart.

## Canvas
#### The right part is the actual position of facility layout
The facility layout is randomly generated in the beginning. When the algorithm stop, you can adjust the position of each block. Following picture is how it works.
![image](https://github.com/CodeGoood/Facility_layout/blob/master/Block%20move.gif)


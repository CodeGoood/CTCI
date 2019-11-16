meta/space.csv
    1) Line 1 ~ 4 are x,y-coordinate of the 4 corners of space
        Format:
          x1,y1
          x2,y2
          x3,y3
          x4,y4
    2) Line 5 is the wind direction (can be any integer in [0,359])
          0 means east wind
         45 means northeast wind
         90 means north wind
        180 means west wind
        270 means south wind

meta/blocks.csv
    Each row should describe the specification of a block.
    The 6 columns are
    1) Block name
        Value: any string WITHOUT COMMAS
        Meaning: the block name to be displayed
    2) Block width
        Value: A real number greater than 0.
        Meaning: 
    3) Block height
        Value: A real number greater than 0.
        Meaning:
    4) Rotatable:
        Value: 0 or 1
        Meaning: If 0, this block won't be rotated. If 1, rotation is acceptable for this block.
    5) Fix X location:
        Value: -1 or a real number in [0,Inf]
        Meaning: If -1, x-coordinate of this block is not fixed and can be adjusted by the algorithm. Otherwise, x-coordinate of this block won't be altered by the algorithm.
    6) Fix Y location
        Value: -1 or a real number in [0,Inf]
        Meaning: If -1, y-coordinate of this block is not fixed and can be adjusted by the algorithm. Otherwise, y-coordinate of this block won't be altered by the algorithm.









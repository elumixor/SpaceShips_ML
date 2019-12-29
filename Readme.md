# Spaceships (Machine Learning)

Unity project to learn some ML


### Goal

Spaceships try to get as far (vertically go top), as they can while going through gates.
Movement is in 2D.


### Genetic Algorithm

Each spaceship has a neural network, which is responsible for spaceship's movement. 
Training is done trough genetic algorithm, inspired by natural selection:

Neural Network Structure:
1. 11 input nodes:

    - Spaceship's position (2)
    - Spaceship's rotation (1)
    - Spaceship's scale (2)
    - Gate's object's position (2)
    - Gate's object's scale (2)
    - Gate width (1)
    - Gate horizontal position (1)

2. 2 Output nodes:

    - Speed: 1 = go forward, -1 = go backwards
    - Steering: 1 = steer right, -1 = steer left

3. Varying structure of FFN (fully connected hidden layers)

    - Simple (8 nodes)
    - Medium (8, 6 nodes)
    - Complex (11, 8, 6 nodes)
        
        
Fitness functions:

- Linear: fitness = 'y' position of the object
- Gate: same as linear but raised to the power of number of gates passed
- Vertical: tries to prohibit unnecessary horizontal movement by subtracting from total passed distance distance passed horizontally
    
Selection functions:

- Best: always chooses the best instance (changes can happen thus only  due to mutation)
- Top two: always chooses best two different instances
- Top two random: chooses randomly wrt fitness
    
Crossover functions:

- Step: changes parent on every new gene, goes through genes sequentially
- StepRandom: for every gene chooses parent randomly
- Average: averages parent's genes
- HalfBestWorse: selects first half of genes from better parent, second - from worse
- HalfWorstBest: selects first half of genes from worse parent, second - from better
- HalfRandomShift: selects half of genes starting from random gene
- FractionByFitness: mixes genes wrt to parents' fitnesses
- FractionRandom: mixes genes in random fraction
    
Mutation:

- Absolute: modifies by fixed amount * random value (-1..1)
- Relative: modifies by gene's value * random value (-1..1)

### Visualizing results

Starting play mode in unity automatically starts training. Generation data (current fitness maximum, minimum, average and medium) 
are written to `../Assets/Logging/data.log` file. Contents can be visualized using [chart.js](https://www.chartjs.org/):

1. Have [node](https://nodejs.org/) installed
1. From project's root `cd Graphs`
1. `npm install; node server.js`
1. Open `Graphs/index.html` in web browser
    
### Analysis

Spaceships master given part of the road through time, but generally fail and overfit 
(this is easily seen by changing first gate's horizontal position) while training models.
Spaceships mostly learn overly complex patters and fail to learn general rules.

Possible causes:

1. Random errors in code :sob:
1. Poorly selected network layout, mostly overly complex
1. Inputs that cause overfitting
1. Selection and crossover are poorly implemented 
1. Poor training environment
1. Small training time and small numbers of simulated instances
1. Inaccurate collision's calculation, unity editor performance

Possible solutions:

1. Do better code testing and implement visualization of NN
1. Custom collisions system, or ignore collisions completely: e.g. destroy object on collision
1. Select NN layout better... somehow...
1. Optimize by using GPU 





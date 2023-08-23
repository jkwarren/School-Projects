"""
by: Jessica Warren and Marco Motroni
date: May 2022

Assingment 10:
This is a game where you use a slider to break all the bricks.

cool things added:
-a restart
-varying the velocity when bouncing
    - if the ball hits the left side of the paddle it decreases the velocity
    (increasing the speed if the ball heads left) and if the ball hits the right
    side, it increases the velocity
- when u is pressed you have turned on the paddle up down function so when
    the up or down keys are pressed, the paddle moves up or down respectivly
- we also added a really bad background song
- we tell the player when they have cleared the board ('you win!')
- we keep score
"""

# Impoert all needed libraries
import pygame
import random
from pygame import mixer

DISPLAY_WIDTH  = 800
DISPLAY_HEIGHT = 600
SURFACE_BKG = (255, 255, 255)  # White

# Initialize game parameters
GAME_FPS = 60  # Number of frames generate for each second

# Values for the ball's parameters
BALL_SPEED_MIN = -200
BALL_SPEED_MAX = 200
BALL_NUMBER = 1
BALL_SIZE = 10

# Values for the paddle's parameters
PADDLE_WIDTH = 80
PADDLE_HEIGHT = 20

# Paramters for text
TXT_COLOR = (0, 0, 255)

#parameters for bricks

BRICK_WIDTH = 80
BRICK_HEIGHT = 30
BRICK_NUMBER = DISPLAY_WIDTH // BRICK_WIDTH
class Ball:
    """
    A class representing a ball on the screen
    
    Attributes
    ----------
    surface : a pygame Surface 
        A pygame Surface type object from the module pygame
    radius : float
        The radius of the ball
    x : float
        The x coordinate of the ball
    y : float
        The y coordinate of the ball
    vx : float
        The x component of the velocity of the ball
    vy : float
        The y component of the velocity of the ball
    color : tuple
        The color of the ball as (red, green, blue)
    
    Methods
    -------
    update()
        Update the ball location and exploding status each time it is called
    collide(other)
        Check for collision with another ball
    swap_velocity(other)
        Bounce this ball off another ball
    draw()
        Draw the ball on the screen
    """
    def __init__(self, surface, radius, x, y, vx, vy, color):
        """
        Parameters
        ----------
        surface : a pygame Surface 
            A pygame Surface type object from the module pygame
        radius : float
            The radius of the ball
        x : float
            The x coordinate of the ball
        y : float
            The y coordinate of the ball
        vx : float
            The x component of the ball velocity
        vy : float
            The y component of the ball velocity
        color : tuple
            The color of the ball as (res, green, blue)
        """
        # Store the parameters in the new object atributes
        self.surface = surface
        self.radius = radius
        self.x = x
        self.y = y
        self.vx = vx
        self.vy = vy
        self.color = color
    
    def draw(self):
        """
        Draw the ball on the screen
        """
        self.rect = pygame.draw.circle(self.surface, self.color, (self.x, self.y), self.radius)
        
#     def collided_with(self, other):
#         """
#         Check if this ball is colliding with another ball
#         
#         Parameters
#         ----------
#         other : ball
#             Another ball to be checked for collision
#         """
#         dx = self.x - other.x
#         dy = self.y - other.y
#         sr = self.radius + other.radius
#         
#         # Check if the balls are overlapping
#         return (dx * dx + dy * dy) < sr * sr
        
#     def swap_velocity(self, other):
#         """
#         Bounce this ball off another ball by swapping velocities
#         
#         Parameters
#         ----------
#         other : ball
#             Another ball to bounce off
#         """
#         # Uses temporary variables to swap values
#         # It also uses tuples to assign two values at a time
#         (temp_vx, temp_vy) = (self.vx, self.vy)
#         (self.vx, self.vy) = (other.vx, other.vy)
#         (other.vx, other.vy) = (temp_vx, temp_vy)

    def update(self, paddle, bricks):
        collisions = []
        collisions_side = []

        """
        Update the ball status:
        - Check if the ball should bounce off the window edges
        - Update position using the velocity
        """
        delta_t = 1/GAME_FPS  # Evaluate the actual time for each fram
        
        # Check if we are passed the screen boundaries
        if (self.x - self.radius < 0) or (self.x + self.radius > DISPLAY_WIDTH):
            self.vx = -self.vx
        if (self.y - self.radius < 0):
            self.vy = -self.vy
            
        #update check if the ball hits the paddle
        if self.rect.colliderect(paddle.rect):
            #checking where on the paddle is hit
            if (self.x <= paddle.left + 20):
                self.vx -= 50
            elif (self.x <= paddle.left + 30):
                self.vx -= 30
                
            if (self.x >= (paddle.left + PADDLE_WIDTH) - 20):
                self.vx += 50
            elif (self.x >= (paddle.left + PADDLE_WIDTH) - 30):
                self.vx += 30
            
            self.vy = -(self.vy)
            if self.vy > 0:
                self.vy += 11
            else:
                self.vy -= 11


        
        #updating the bricks
        
        #changing it so only one brick is hit


        for brick in bricks:
              
            if self.rect.colliderect(brick.rect) == True and brick.visible == True:
                
                if (self.x <= brick.x) or (self.x >= (brick.x + BRICK_WIDTH)):

                    collisions_side.append(True)
                    brick.visible = False


                elif (self.x > brick.x) and (self.x < brick.x + BRICK_WIDTH):
                    
                    collisions.append('True')
                    brick.visible = False

            
        if 'True' in collisions:
            self.vy = -self.vy
        if True in collisions_side:
            self.vx = -self.vx
        # Updates the location using velocity and time
        self.x += self.vx * delta_t
        self.y += self.vy * delta_t

class Paddle:
    """
    a class representing a paddle on the screen.
        Attributes
    ----------
    surface : a pygame Surface 
        A pygame Surface type object from the module pygame
    
    width:
        The width of the paddle
    height:
        the height of the paddle
    color : tuple
        The color of the paddle as (red, green, blue)
    
    Methods
    -------
    draw()
        Draw the paddle on the screen
    """
    def __init__(self, surface, width, height, color):
        """
        Parameters
        ----------
        surface : a pygame Surface 
            A pygame Surface type object from the module pygame
         width:
            The width of the paddle
        height:
            the height of the paddle
        color : tuple
            The color of the paddle as (res, green, blue)
        """
        self.surface = surface
        self.width = width
        self.height = height
        self.color = color
        self.left = (DISPLAY_WIDTH // 2) - width//2
        self.top = DISPLAY_HEIGHT - 2 * height
    def draw (self):
        """ it draws the rectangle"""
        rectangle = pygame.Rect(self.left, self.top, self.width, self.height)
        self.rect = pygame.draw.rect(self.surface, self.color, rectangle)
        

class Brick:
    def __init__ (self, surface, x, y, width, height, color):
        self.surface = surface
        self.x = x
        self.y = y
        self.width = width
        self.height = height
        self.color = color

        self.visible = True
    def draw (self):
        """ it draws the brick"""
        if self.visible == True:
            brick = pygame.Rect(self.x, self.y, self.width, self.height)
            self.rect = pygame.draw.rect(self.surface, self.color, brick)

    
class Game:
    """
    A class implementing the multiple ball scene
    
    Attributes
    ----------
    surface : Surface
        A Surface type object from the module pygame
    clock : Clock
        A Clock type object from the module pygame
    balls : list
        A list containing all the balls objects
    
    Methods
    -------
    initialize_objects()
        Create the gamefield by adding the balls
    run()
        Determines the game dynamic frame by frame (one frame for each call)
    """
    def __init__(self):
        """
        Initialize the scene
        """
        # Initialize all the components needed by pygame
        pygame.init()
        # Create a surface that we can use to draw objects
        self.surface = pygame.display.set_mode((DISPLAY_WIDTH, DISPLAY_HEIGHT))
        # Create a clock that we can use to keep track of time 
        self.clock = pygame.time.Clock()
        # Create a font that we can use to write on the screen
        self.font = pygame.font.SysFont(None, 28)

        
        # Call our method to initialize all tje objects (balls)
        self.initialize_objects()
        pygame.key.set_repeat(1, 25)

    def initialize_objects(self):
        """
        Initialize all the objects in the scene
        """
#         self.balls = []  # A list to store the balls
        self.bricks = []
        for i in range(BALL_NUMBER):
            # Random location within the screen
            x = (DISPLAY_WIDTH// 2)
            y = (DISPLAY_HEIGHT //2)
            
            # Random velocity
            vx = random.randint(BALL_SPEED_MIN, BALL_SPEED_MAX)
            vy = -200
#             vx = 10
#             vy = 0
            
            # Random color
            red = random.randint(0, 255)
            green = random.randint(0, 255)
            blue = random.randint(0, 255)
            color = (red, green, blue)
            
            # Create a Ball object and store it in the list
#             ball = Ball(self.surface, BALL_SIZE, x, y, vx, vy, (0,0,0))
#             self.balls.append(ball)
            
            self.ball = Ball(self.surface, BALL_SIZE, x, y, vx, vy, (0,0,0))
            
            
        width = PADDLE_WIDTH
        height = PADDLE_HEIGHT
        color = (0,0,255)
        self.paddle = Paddle(self.surface, width, height, color)
        
        #for the brick
        x = 0
        y = 0
        for i in range(BRICK_NUMBER):
            if i > 0:
                x += BRICK_WIDTH
            #y += BRICK_HEIGHT
            color = (255, 0, 0)
            brick = Brick (self.surface, x, y, BRICK_WIDTH, BRICK_HEIGHT, color)
            self.bricks.append(brick)
            
        x = 0
        y = BRICK_HEIGHT
        for i in range(BRICK_NUMBER):
            if i > 0:
                x += BRICK_WIDTH
            #y += BRICK_HEIGHT
            color = (255, 0, 0)
            brick = Brick (self.surface, x, y, BRICK_WIDTH, BRICK_HEIGHT, color)
            self.bricks.append(brick)
            #extra brick
#         color = (255, 0, 0)
#         brick = Brick (self.surface, (DISPLAY_WIDTH// 2) + 30, (DISPLAY_HEIGHT //2), BRICK_WIDTH, BRICK_HEIGHT, color)
#         self.bricks.append(brick)
        

    def run(self):
        """
        Determines the game dynamic frame by frame
        """
        # Infinite loop (one frame for each iteration)
        updown = False
        mixer.init()
        mixer.music.load ('216.mp3')
        mixer.music.play()
        while True:
            
            # Check for events
            event = pygame.event.poll()  # Grab the next event
            # Check if the event is a QUIT event (close the window)
            if event.type == pygame.QUIT:
                break
            # Check if the user pushed a key
            if event.type == pygame.KEYDOWN:
                # Check if the key that was pushed was the 'q' key
                if event.key == pygame.K_q:
                    break
                #left and right
                if event.key == pygame.K_LEFT:
                    self.paddle.left -= 10
                if event.key == pygame.K_RIGHT:
                    self.paddle.left += 10
                    
                    
                #up and down
                #this turns on the up and down function
                if event.key == pygame.K_u:
                    updown = True
                    
                
                if event.key == pygame.K_DOWN and updown == True:
                    self.paddle.top += 10
                if event.key == pygame.K_UP and updown == True:
                    self.paddle.top -= 10
            
            #restarts the game
                if event.key == pygame.K_r:
                    game = Game()
                    game.run()
            
            # Refresh the background
            self.surface.fill(SURFACE_BKG)

            # Check for collisions, update, and draw
            for i in range(len(self.bricks)):
                b1 = self.bricks[i]
#                 for j in range(i + 1, len(self.balls)):
#                     b2 = self.balls[j]
#                     # Check for collisions
#                     if b1.collided_with(b2):
#                         b1.swap_velocity(b2)
                b1.draw()

            self.ball.draw()
            self.paddle.draw()
            
            self.ball.update(self.paddle, self.bricks)
            
            #to win
            brick_list= [1 for brick in self.bricks if brick.visible == False]
            
            if sum((brick_list)) == 2 * BRICK_NUMBER:
                text = self.font.render('You Win!!', True, TXT_COLOR)
                self.surface.blit(text, (20, 0.5 * DISPLAY_HEIGHT))
                pygame.display.update()
                pygame.time.delay(4000)
                game1 = Game()
                game1.run()
            
                
            
            
            if self.ball.y > DISPLAY_HEIGHT:
                break
            
            
            # Write a string on the screen
            text = self.font.render('You Have Hit ' + str(sum(brick_list)) + ' Bricks', True, TXT_COLOR)
            self.surface.blit(text, (10, 0.20 * DISPLAY_HEIGHT))
            
            text = self.font.render('q = quit, r = restart, u = allows you to move paddle in all directions', True, (0, 200, 0))
            self.surface.blit(text, (8, 0.97 * DISPLAY_HEIGHT))


                                    
            # Update the screen
            pygame.display.update()
            self.clock.tick(GAME_FPS)

        # Quit pygame
        pygame.quit()
    
# Run the game
game = Game()
game.run()


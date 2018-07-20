import 'phaser';
import TestScene from './scenes/TestScene';

const config: GameConfig = {
    type: Phaser.AUTO,
    parent: '#gameContainer',
    width: 640,
    height: 480,
    resolution: 1, 
    backgroundColor: "#EDEEC9",
    scene: [
      TestScene
    ]
};

new Phaser.Game(config);
module Stars

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Content
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework.Input
open System

type Star = 
    { x : float;
      y : float;
      dx : float;
    }

let width = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width
let height = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height
let r = Random()

let moveStar (gameTime:GameTime) star =
    let time = gameTime.ElapsedGameTime.TotalSeconds
    let x' = star.x - star.dx * time
    if x' < 0.0 then { x = (float)width; 
                       y = r.NextDouble() * (float)height; 
                       dx = r.NextDouble() * 200.0; }
        else { star with x = star.x - star.dx * time; }

let starsUpdate (gameTime : GameTime) state =
    let mover = moveStar gameTime
    match state with
        | None -> List.init 1000 (fun index -> { x = r.NextDouble() * (float)width; 
                                                 y = r.NextDouble() * (float)height; 
                                                 dx = r.NextDouble() * 200.0; })
        | Some state -> List.map mover state

let drawPoint (spriteBatch : SpriteBatch) pixel star =
    spriteBatch.Draw(pixel, Rectangle((int)star.x, (int)star.y, 1, 1), Color.White)

let starsDraw (graphics:GraphicsDevice) (spriteBatch:SpriteBatch) pixel state =
    do graphics.Clear Color.Black
    spriteBatch.Begin()
    List.iter (drawPoint spriteBatch pixel) state
    spriteBatch.End()

﻿module Plasma

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Content
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework.Input

type Colour =
    { r : int;
      g : int;
      b : int }

type Location = 
    { x : int;
      y : int }

type PlasmaPoint =
    { location: Location;
      v : float; }

type ColourPoint =
    { location : Location;
      colour : Colour }

let pixelSize = 10
let width = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / pixelSize
let height = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / pixelSize
let width' = (float)width
let height' = (float)height

let pointList =
    [ for x in 0..width do
          yield [for y in 0..height do
                     yield { x = x; y = y; } ] ]
    |> Seq.concat
    |> List.ofSeq

let plasmaOne tick point =
    let x' = (float)point.x / width' * 2.0
    let y' = (float)point.y / height' * 2.0
    let cx = x' + 0.5 * sin ( tick / 5.0 )
    let cy = y' + 0.5 * cos ( tick / 3.0 )
    { location = point;
      v = sin ( x' * 2.0 + tick ) +
          sin ( 2.0 * ( x' * sin ( tick / 2.0 ) + y' * cos ( tick / 3.0 )) + tick ) +
          sin ( sqrt ( 10.0 * ( cx*cx + cy*cy ) ) + 0.5 + tick); }

let colour ( v : PlasmaPoint ) =
    { location = v.location; 
      colour = { r = (int)(( sin ( v.v * (float)MathHelper.Pi ) + 1.0 ) * 128.0 );
                 g = (int)(( cos ( v.v * (float)MathHelper.Pi ) + 1.0 ) * 128.0 );
                 b = 0; } }

let plasma tick =
    let vList = List.map (plasmaOne tick) pointList
    List.map colour vList

let drawPoint (spriteBatch : SpriteBatch) pixel point =
    spriteBatch.Draw(pixel, Rectangle(point.location.x * pixelSize, point.location.y * pixelSize, pixelSize, pixelSize), Color(point.colour.r, point.colour.g, point.colour.b))

type PlasmaEffect (spriteBatch:SpriteBatch, pixel) as this =
    
    let spriteBatch = spriteBatch
    let pixel = pixel
    let mutable plasmaState = None

    member this.Update (gameTime : GameTime) =
        let keyState = Keyboard.GetState()
        match keyState.IsKeyDown(Keys.Escape) with
            | true -> do ()
            | false -> let diff = (float)gameTime.TotalGameTime.TotalSeconds
                       do plasmaState <- Some (plasma diff)
                       do ()
        ()

    member this.Draw gameTime =
        match plasmaState with
            | None -> ()
            | Some state -> do
                spriteBatch.Begin()
                List.iter (drawPoint spriteBatch pixel) state
                spriteBatch.End()
        ()

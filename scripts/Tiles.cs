/*using Godot;
using System;
using PaintColor;

public class Tiles {
    public static Tile blueTile = new Tile(3,6,9);
    public static Tile yellowTile = new Tile(4,7,10);
    public static Tile neutralTile = new Tile(5,8,11);

    public static Tile[] tiles = new Tile[] { blueTile, yellowTile, neutralTile };

    public void isPaintable(int id){
        for(int i = 0; i < tiles.Length; i++){
            if (tiles[i].containsID(id)) return true;
        }
        return false;
    }

    public PaintColor getColor(int id){
        for(int i = 0; i < tiles.Length; i++){
            if (tiles[i].containsID(id)) return tiles[i].color;
        }
        return null;
    }

    public int getEquivalentID(int id, PaintColor color){
         for(int i = 0; i < tiles.Length; i++){
            if (tiles[i].containsID(id)) {
              
            }
        }
    }

}*/
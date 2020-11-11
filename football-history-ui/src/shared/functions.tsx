import React from "react";

function isString(x: any): x is string {
    return typeof x === "string";
}

function isNumber(x: any): x is number {
    return typeof x === "number";
}

function getLeagueStatusColor(status: string | null) {
    switch (status) {
        case "Champions":
            return Color.Green;
        case "Promoted":
            return Color.Blue;
        case "Relegated":
            return Color.Red;
        case "PlayOffs":
            return Color.Yellow;
        case "PlayOff Winner":
            return Color.Blue;
        case "Relegation PlayOffs":
            return Color.Yellow;
        case "Relegated - PlayOffs":
            return Color.Red;
        case "Failed Re-election":
            return Color.Red;
        case "Re-elected":
            return Color.Yellow;
        default:
            return null;
    }
}

enum Color {
    Green = "#75B266",
    Blue = "#7FBFBF",
    Red = "#B26694",
    Yellow = "#BFA67F",
    Grey = "#CCCCCC",
}

export { isString, isNumber, getLeagueStatusColor, Color };

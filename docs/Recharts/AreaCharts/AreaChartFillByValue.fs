﻿[<RequireQualifiedAccess>]
module Samples.Recharts.AreaCharts.AreaChartFillByValue

open Feliz
open Feliz.Recharts
open Fable.Core.Experimental

type Point = { name: string; uv: int; pv: int }

let data = [
    { name = "Page A"; uv = 4000; pv = 2400 }
    { name = "Page B"; uv = 3000; pv = 1398 }
    { name = "Page C"; uv = -1000; pv = 9800 }
    { name = "Page D"; uv = 500; pv = 3908 }
    { name = "Page E"; uv = -2000; pv = 4800 }
    { name = "Page F"; uv = -250; pv = 3800 }
    { name = "Page G"; uv = 3490; pv = 4300 }
]

let getOffset (data: Point list) =
    match data with
    | [ ] -> 0.0
    | points ->
        let dataMax = points |> List.maxBy (fun point -> point.uv)
        let dataMin = points |> List.minBy (fun point -> point.uv)

        if dataMax.uv <= 0
        then 0.0
        elif dataMin.uv >= 0
        then 1.0
        else float dataMax.uv / (float dataMax.uv - float dataMin.uv)

let getGradientDefinition (gradientId: string) (data: Point list) =
    let offset = getOffset data
    Svg.defs [
        Svg.linearGradient [
            svg.id gradientId
            svg.x1 0; svg.x2 0
            svg.y1 0; svg.y2 1
            svg.children [
                Svg.stop [
                    svg.offset offset
                    svg.stopColor "green"
                    svg.stopOpacity 1.0
                ]

                Svg.stop [
                    svg.offset offset
                    svg.stopColor "red"
                    svg.stopOpacity 1.0
                ]
            ]
        ]
    ]

let chart = React.functionComponent(fun () -> [
    Recharts.areaChart [
        areaChart.height 400
        areaChart.width 500
        areaChart.data data
        areaChart.margin(top=10, right=30)
        areaChart.children [
            Recharts.cartesianGrid [ cartesianGrid.strokeDasharray(3, 3) ]
            Recharts.xAxis [ xAxis.dataKey (fun point -> point.name) ]
            Recharts.yAxis [ ]
            Recharts.tooltip [ ]
            getGradientDefinition "splitColor" data
            Recharts.area [
                area.monotone
                area.dataKey (fun point -> point.uv)
                area.stroke color.black
                area.fill "url(#splitColor)"
            ]
        ]
    ]
])
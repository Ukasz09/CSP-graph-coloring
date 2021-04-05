from typing import *
import matplotlib.pyplot as plt
import numpy as np


def _draw_edge(fst_point: Tuple[int, int], snd_point: Tuple[int, int], edge_color='black'):
    x_cord = [fst_point[0], snd_point[0]]
    y_cord = [fst_point[1], snd_point[1]]
    plt.plot(x_cord, y_cord, color=edge_color)


def _draw_edges(edges: List[Tuple[Tuple[int, int], Tuple[int, int]]]):
    for edge in edges:
        fst_point = edge[0]
        snd_point = edge[1]
        _draw_edge(fst_point, snd_point)


def _draw_nodes(nodes: List[Tuple[int, int]], colors: List[str], node_size=150):
    for i in range(len(nodes)):
        point = nodes[i]
        plt.scatter(point[0], point[1], color=colors[i], s=node_size)


def _plot_graph(nodes: List[Tuple[int, int]], edges: List[Tuple[Tuple[int, int], Tuple[int, int]]], colors: List[str]):
    plot_title_scheme = 'NodesQty = {nodes_qty}, EdgesQty = {edgesQty}, ColorsQty = {colorsQty}'
    distinct_colors_qty = len(np.unique(colors))
    plot_title = plot_title_scheme.format(nodes_qty=len(nodes), edgesQty=len(edges), colorsQty=distinct_colors_qty)
    plt.title(plot_title)
    _draw_nodes(nodes, colors)
    _draw_edges(edges)


if __name__ == "__main__":
    # Prepare data
    _nodes = [(0, 3), (1, 3), (2, 1), (3, 4)]
    _colors = ['red', 'green', 'blue', 'red']
    _edges = [
        (_nodes[0], _nodes[1]),
        (_nodes[0], _nodes[2]),
        (_nodes[0], _nodes[3]),
    ]

    # Plot graph
    _plot_graph(_nodes, _edges, _colors)
    plt.show()
    exit(0)

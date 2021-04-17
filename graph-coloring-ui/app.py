import sys
from typing import *
import matplotlib.pyplot as plt
import numpy as np
import json


def _draw_edge(fst_point: Tuple[int, int], snd_point: Tuple[int, int], edge_color='black'):
    x_cord = [fst_point[0], snd_point[0]]
    y_cord = [fst_point[1], snd_point[1]]
    plt.plot(x_cord, y_cord, color=edge_color)


def _draw_edges(edges: List[Tuple[Tuple[int, int], Tuple[int, int]]]):
    for edge in edges:
        fst_point = edge[0]
        snd_point = edge[1]
        _draw_edge(fst_point, snd_point)


def _draw_nodes(nodes: List[Tuple[int, int]], colors: Dict[Tuple[int, int], str], node_size=100):
    for i in range(len(nodes)):
        point = nodes[i]
        if point in colors:
            color = colors[point]
        else:
            color = "black"
        plt.scatter(point[0], point[1], color=color, s=node_size)
        plt.annotate(f'({point[0]},{point[1]})',
                     (point[0], point[1]),
                     textcoords="offset points",
                     xytext=(0, 15),
                     bbox=dict(boxstyle="round", alpha=0.1),
                     ha='center')


def _plot_graph(nodes: List[Tuple[int, int]], edges: List[Tuple[Tuple[int, int], Tuple[int, int]]],
                colors: Dict[Tuple[int, int], str]):
    plot_title_scheme = 'NodesQty = {nodes_qty}, EdgesQty = {edgesQty}, ColorsQty = {colorsQty}'
    distinct_colors_qty = len(np.unique(list(colors.values())))
    plot_title = plot_title_scheme.format(nodes_qty=len(nodes), edgesQty=len(edges), colorsQty=distinct_colors_qty)
    plt.title(plot_title, pad=25)
    _draw_nodes(nodes, colors)
    _draw_edges(edges)


def _read_graph(filepath: str):
    """
    @:return List[Tuple[Tuple[int, int], Tuple[int, int]]]
    """
    with open(filepath) as json_file:
        data = json.load(json_file)
        # Parsing list into tuples
        tuple_data = [tuple([tuple(nodes_list[0]), tuple(nodes_list[1])]) for nodes_list in data]
        return tuple_data


def _read_colors(filepath: str) -> List[Dict[any, str]]:
    with open(filepath) as json_file:
        results_list = json.load(json_file)
        return results_list


def _extract_unique_nodes_from_edges(edges):
    points = []
    for edge in edges:
        points.append(edge[0])
        points.append(edge[1])
    nodes = list(set(points))
    return nodes


def _plot_result(_nodes: List[Tuple[int, int]], _edges: List[Tuple[Tuple[int, int], Tuple[int, int]]], _colors_raw):
    colors = {}
    for obj in _colors_raw:
        node = (obj["node"][0], obj["node"][1])
        color = obj["color"]
        colors[node] = color
    # Plot graph
    _plot_graph(_nodes, _edges, colors)
    plt.show()


if __name__ == "__main__":
    _edges = _read_graph('/home/ukasz09/Documents/OneDrive/Uczelnia/Semestr_VI/SI-L/2/graph-coloring-ui/graph.json')
    _nodes = _extract_unique_nodes_from_edges(_edges)
    _colors_raw = _read_colors(
        '/home/ukasz09/Documents/OneDrive/Uczelnia/Semestr_VI/SI-L/2/graph-coloring-ui/solution.json')
    allSolutionsFlag = "all"
    if len(sys.argv) > 1 and sys.argv[1] == allSolutionsFlag:
        for _colors_data in _colors_raw:
            _plot_result(_nodes, _edges, _colors_data)
    else:
        _plot_result(_nodes, _edges, _colors_raw)
    exit(0)

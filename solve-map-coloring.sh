#!/bin/bash

./csp-problem/csp-problem/csp-problem/bin/Debug/netcoreapp3.1/csp-problem map ${1}
python3 ./graph-coloring-ui/app.py ${1} 

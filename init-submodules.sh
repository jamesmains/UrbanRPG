#!/bin/bash

# Script to initialize and update Git submodules
echo "Initializing submodules..."

git submodule init
git submodule update --recursive

echo "Submodules initialized and updated!"


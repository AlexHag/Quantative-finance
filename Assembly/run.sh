#!/bin/bash

set -e

if [ $# -ne 1 ]; then
  echo "Usage: $0 <filename>"
  exit 1
fi

input_file="$1"
base_name="${input_file%.*}"

nasm -f elf64 ${input_file} -o ${base_name}.o
ld -m elf_x86_64 -s -o ${base_name} ${base_name}.o
./${base_name}
echo "Exit code: $?"

SYS_read equ 0
SYS_write equ 1
SYS_open equ 2
SYS_close equ 3
SYS_exit equ 60

STDOUT equ 1
STDERR equ 2

O_RDONLY equ 0

%macro read 3
    mov rax, SYS_read
    mov rdi, %1
    mov rsi, %2
    mov rdx, %3
    syscall
%endmacro

%macro write 3
    mov rax, SYS_write
    mov rdi, %1
    mov rsi, %2
    mov rdx, %3
    syscall
%endmacro

%macro open 2
    mov rax, SYS_open,
    mov rdi, %1
    mov rsi, %2
    syscall
%endmacro

%macro close 1
    mov rax, SYS_close
    mov rdi, %1
    syscall
%endmacro

%macro exit 1
    mov rax, SYS_exit
    mov rdi, %1
    syscall
%endmacro

section .text
    global _start

_start:
    write STDOUT, start_msg, start_msg_len
    cmp rax, 0
    jl error

    open file_path, O_RDONLY
    cmp rax, 0
    jl error
    mov [file_descriptor], rax

    read [file_descriptor], buffer, buffer_size
    cmp rax, 0
    jl error
    mov [bytes_read], rax

    write STDOUT, buffer, bytes_read
    cmp rax, 0
    jl error

    exit 0

error:
    write STDERR, error_msg, error_msg_len
    exit 1

section .data
    start_msg db "Starting...", 0xa
    start_msg_len equ $ - start_msg
    error_msg db "Error...", 0xa
    error_msg_len equ $ - error_msg

    file_path db "./hello.txt", 0
    buffer db 1024 dup(0)
    buffer_size equ $ - buffer

section .bss
    file_descriptor resq 1
    bytes_read resq 1

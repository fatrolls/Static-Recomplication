# Static-Recomplication
x86 Static Recomplication (Convert Compiled EXE Functions to Runnable/Callable Code in C++) in vb.net

Automatically creates a .cpp / .h file for every function found in EXE.

All Data sections like .rdata / .data of EXE file gets put into a huge array in `rdata_section.h` / `data_section.h`

This project is very good if you have a game you want to hack and the encryption is crazy as hell and instead of converting it to C/C++ code using IDA Pro (F5 Key) then fixing it so it runs.. this project would emulate a x86 CPU internelly and run your ASM Code in a sandbox, then output your results you want, saves hours and weeks of work on crazy encryptions just run the Translator and bam you are done to use it in any of your C++ projects. Automatically detects functions inside other functions and rips them all out, only problem would be the imports you would need to do this by hand dump your imports using Scylla (Save Tree function) to get all addresses. Then modify the base.h to add functionatility (for more information about imports look at base.h / ios.h files)

Original x86 CPU emulator and FPU too (floating-point instructions) was written in C++ with a python script to convert EXE to .cpp/.h files to be run in C++. I converted it to vb.net for my needs as I find it easier to work in vb.net much easier to debug problems if something crashes without Access violations / segmentation faults you get with C++.

Example of translated ASM code back to Runnable C++ code.
```
#include "main.h"
#include "functions.h"
#include "sections.h"
#include <iostream>

void sub_557380()
{
    cpu.push_dword(cpu.reg[EBP]); // [557380] push ebp
    cpu.reg[EBP] = cpu.reg[ESP]; // [557381] mov ebp,esp
    cpu.push_dword(cpu.reg[ECX]); // [557383] push ecx
    mem.write_word(cpu.reg[EBP]+(-0x4), 65535U); // [557384] mov word [ebp-0x4],0xffff
    cpu.get_reg16(AX) = mem.read_word(cpu.reg[EBP]+(-0x4)); // [55738A] mov ax,[ebp-0x4]
    cpu.get_reg8(CL) = cpu.get_reg8(CL) | 255U; // [55738E] or cl,0xff
    mem.write_word(mem.translate(data_section+8920), cpu.get_reg16(AX)); // [557391] mov [0x5842d8],ax
    mem.write_byte(mem.translate(data_section+8922), cpu.get_reg8(CL)); // [557397] mov [0x5842da],cl
    mem.write_byte(mem.translate(data_section+8923), 0U); // [55739D] mov byte [0x5842db],0x0
    cpu.reg[ESP] = cpu.reg[EBP]; // [5573A4] mov esp,ebp
    cpu.pop_dword(&cpu.reg[EBP]); // [5573A6] pop ebp
    cpu.reg[ESP] = cpu.reg[ESP]+4; // [5573A7] ret
    return; // [5573A7] ret
}
```

Example using imports translated ASM code back to Runnable C++ code.

```
// ostream_writestr
void sub_4C6970()
{
    pop_ret();
    uint32_t stream = cpu.get_dword(0);
    std::stringstream & str = *current_stream;
    uint32_t val = cpu.get_dword(4);
    basic_string_char * v = (basic_string_char*)mem.translate(val);
    // std::cout << "ostream_writestr: " << v->str() << std::endl;
    str << v->str();
    set_ret(stream);
}

// copy_iostream_to_str
void sub_4D8B70()
{
    pop_ret();
    uint32_t stream = get_self();
    std::stringstream & str = *current_stream;
    uint32_t stdstring = cpu.pop_dword();
    // need to initialize str since it hasn't been done already
    ((basic_string_char*)mem.translate(stdstring))->reset();

    // move string to guest space
    std::string res = str.str();
    uint32_t str_guest = mem.heap_alloc(res.size());
    memcpy(mem.translate(str_guest), &res[0], res.size());

    // use address 401A00, which is create_string_from_cstr
    // signature: string * __thiscall f(char* str, int size)
    get_self() = stdstring;
    cpu.push_dword(res.size());
    cpu.push_dword(str_guest);
    add_ret();
    sub_401A00();

    mem.heap_dealloc(str_guest);

    set_ret(stdstring);

    // std::cout << "copy_iostream_to_str: " << res << std::endl;
}
```

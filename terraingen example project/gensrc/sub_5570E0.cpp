/*
    Copyright (c) Mathias Kaerlev 2013-2014.

    This file is part of cuwo.

    cuwo is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    cuwo is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with cuwo.  If not, see <http://www.gnu.org/licenses/>.
*/

#include "main.h"
#include "functions.h"
#include "sections.h"
#include <iostream>

void sub_5570E0()
{
    cpu.push_dword(cpu.reg[EBP]); // [5570E0] push ebp
    cpu.reg[EBP] = cpu.reg[ESP]; // [5570E1] mov ebp,esp
    cpu.push_dword(cpu.reg[ECX]); // [5570E3] push ecx
    mem.write_word(cpu.reg[EBP]+(-0x4), 51400U); // [5570E4] mov word [ebp-0x4],0xc8c8
    cpu.get_reg16(AX) = mem.read_word(cpu.reg[EBP]+(-0x4)); // [5570EA] mov ax,[ebp-0x4]
    mem.write_word(mem.translate(data_section+8836), cpu.get_reg16(AX)); // [5570EE] mov [0x584284],ax
    mem.write_word(mem.translate(data_section+8838), 456U); // [5570F4] mov word [0x584286],0x1c8
    cpu.reg[ESP] = cpu.reg[EBP]; // [5570FD] mov esp,ebp
    cpu.pop_dword(&cpu.reg[EBP]); // [5570FF] pop ebp
    cpu.reg[ESP] = cpu.reg[ESP]+4; // [557100] ret
    return; // [557100] ret
}

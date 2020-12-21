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

void sub_557440()
{
    cpu.push_dword(cpu.reg[EBP]); // [557440] push ebp
    cpu.reg[EBP] = cpu.reg[ESP]; // [557441] mov ebp,esp
    cpu.push_dword(cpu.reg[ECX]); // [557443] push ecx
    mem.write_word(cpu.reg[EBP]+(-0x4), 51400U); // [557444] mov word [ebp-0x4],0xc8c8
    cpu.get_reg16(AX) = mem.read_word(cpu.reg[EBP]+(-0x4)); // [55744A] mov ax,[ebp-0x4]
    mem.write_word(mem.translate(data_section+8944), cpu.get_reg16(AX)); // [55744E] mov [0x5842f0],ax
    mem.write_word(mem.translate(data_section+8946), 456U); // [557454] mov word [0x5842f2],0x1c8
    cpu.reg[ESP] = cpu.reg[EBP]; // [55745D] mov esp,ebp
    cpu.pop_dword(&cpu.reg[EBP]); // [55745F] pop ebp
    cpu.reg[ESP] = cpu.reg[ESP]+4; // [557460] ret
    return; // [557460] ret
}

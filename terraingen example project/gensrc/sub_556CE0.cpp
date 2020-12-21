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

void sub_556CE0()
{
    cpu.push_dword(cpu.reg[EBP]); // [556CE0] push ebp
    cpu.reg[EBP] = cpu.reg[ESP]; // [556CE1] mov ebp,esp
    cpu.push_dword(cpu.reg[ECX]); // [556CE3] push ecx
    mem.write_word(cpu.reg[EBP]+(-0x4), 51400U); // [556CE4] mov word [ebp-0x4],0xc8c8
    cpu.get_reg16(AX) = mem.read_word(cpu.reg[EBP]+(-0x4)); // [556CEA] mov ax,[ebp-0x4]
    mem.write_word(mem.translate(data_section+7608), cpu.get_reg16(AX)); // [556CEE] mov [0x583db8],ax
    mem.write_word(mem.translate(data_section+7610), 456U); // [556CF4] mov word [0x583dba],0x1c8
    cpu.reg[ESP] = cpu.reg[EBP]; // [556CFD] mov esp,ebp
    cpu.pop_dword(&cpu.reg[EBP]); // [556CFF] pop ebp
    cpu.reg[ESP] = cpu.reg[ESP]+4; // [556D00] ret
    return; // [556D00] ret
}

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

void sub_556CB0()
{
    cpu.push_dword(cpu.reg[EBP]); // [556CB0] push ebp
    cpu.reg[EBP] = cpu.reg[ESP]; // [556CB1] mov ebp,esp
    cpu.push_dword(cpu.reg[ECX]); // [556CB3] push ecx
    mem.write_word(cpu.reg[EBP]+(-0x4), 65535U); // [556CB4] mov word [ebp-0x4],0xffff
    cpu.get_reg16(AX) = mem.read_word(cpu.reg[EBP]+(-0x4)); // [556CBA] mov ax,[ebp-0x4]
    cpu.get_reg8(CL) = cpu.get_reg8(CL) | 255U; // [556CBE] or cl,0xff
    mem.write_word(mem.translate(data_section+7604), cpu.get_reg16(AX)); // [556CC1] mov [0x583db4],ax
    mem.write_byte(mem.translate(data_section+7606), cpu.get_reg8(CL)); // [556CC7] mov [0x583db6],cl
    mem.write_byte(mem.translate(data_section+7607), 0U); // [556CCD] mov byte [0x583db7],0x0
    cpu.reg[ESP] = cpu.reg[EBP]; // [556CD4] mov esp,ebp
    cpu.pop_dword(&cpu.reg[EBP]); // [556CD6] pop ebp
    cpu.reg[ESP] = cpu.reg[ESP]+4; // [556CD7] ret
    return; // [556CD7] ret
}

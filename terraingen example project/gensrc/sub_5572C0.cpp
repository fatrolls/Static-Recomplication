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

void sub_5572C0()
{
    cpu.push_dword(cpu.reg[EBP]); // [5572C0] push ebp
    cpu.reg[EBP] = cpu.reg[ESP]; // [5572C1] mov ebp,esp
    cpu.push_dword(cpu.reg[ECX]); // [5572C3] push ecx
    mem.write_word(cpu.reg[EBP]+(-0x4), 65535U); // [5572C4] mov word [ebp-0x4],0xffff
    cpu.get_reg16(AX) = mem.read_word(cpu.reg[EBP]+(-0x4)); // [5572CA] mov ax,[ebp-0x4]
    cpu.get_reg8(CL) = cpu.get_reg8(CL) | 255U; // [5572CE] or cl,0xff
    mem.write_word(mem.translate(data_section+8876), cpu.get_reg16(AX)); // [5572D1] mov [0x5842ac],ax
    mem.write_byte(mem.translate(data_section+8878), cpu.get_reg8(CL)); // [5572D7] mov [0x5842ae],cl
    mem.write_byte(mem.translate(data_section+8879), 130U); // [5572DD] mov byte [0x5842af],0x82
    cpu.reg[ESP] = cpu.reg[EBP]; // [5572E4] mov esp,ebp
    cpu.pop_dword(&cpu.reg[EBP]); // [5572E6] pop ebp
    cpu.reg[ESP] = cpu.reg[ESP]+4; // [5572E7] ret
    return; // [5572E7] ret
}

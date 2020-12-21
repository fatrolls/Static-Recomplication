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

void sub_556980()
{
    cpu.push_dword(cpu.reg[EBP]); // [556980] push ebp
    cpu.reg[EBP] = cpu.reg[ESP]; // [556981] mov ebp,esp
    cpu.push_dword(cpu.reg[ECX]); // [556983] push ecx
    mem.write_word(cpu.reg[EBP]+(-0x4), 51400U); // [556984] mov word [ebp-0x4],0xc8c8
    cpu.get_reg16(AX) = mem.read_word(cpu.reg[EBP]+(-0x4)); // [55698A] mov ax,[ebp-0x4]
    mem.write_word(mem.translate(data_section+7492), cpu.get_reg16(AX)); // [55698E] mov [0x583d44],ax
    mem.write_word(mem.translate(data_section+7494), 456U); // [556994] mov word [0x583d46],0x1c8
    cpu.reg[ESP] = cpu.reg[EBP]; // [55699D] mov esp,ebp
    cpu.pop_dword(&cpu.reg[EBP]); // [55699F] pop ebp
    cpu.reg[ESP] = cpu.reg[ESP]+4; // [5569A0] ret
    return; // [5569A0] ret
}

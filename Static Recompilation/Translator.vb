Imports System.IO
Imports System.Reflection

Module Translator
    Public frmMain As New frmMain

    Public MODE_32 As Integer = 0
    Public MODE_16 As Integer = 1

    Public reg_table As New Dictionary(Of UInteger, String()) From { _
        {OpCodes.REG_GEN_DWORD, New String() {"EAX", "ECX", "EDX", "EBX", "ESP", "EBP", "ESI", "EDI"}}, _
        {OpCodes.REG_GEN_WORD, New String() {"AX", "CX", "DX", "BX", "SP", "BP", "SI", "DI"}}, _
        {OpCodes.REG_GEN_BYTE, New String() {"AL", "CL", "DL", "BL", "AH", "CH", "DH", "BH"}}, _
        {OpCodes.REG_SEGMENT, New String() {"ES", "CS", "SS", "DS", "FS", "GS"}}, _
        {OpCodes.REG_XMM, New String() {"XMM0", "XMM1", "XMM2", "XMM3", "XMM4", "XMM5", "XMM6", "XMM7"}}, _
        {OpCodes.REG_MMX, New String() {"MM0", "MM1", "MM2", "MM3", "MM4", "MM5", "MM6", "MM7"}}, _
        {OpCodes.REG_FPU, New String() {"ST0", "ST1", "ST2", "ST3", "ST4", "ST5", "ST6", "ST7"}} _
    }

    Public reg_ref_table As New Dictionary(Of UInteger, String) From { _
        {OpCodes.REG_GEN_DWORD, "cpu.reg[{0}]"}, _
        {OpCodes.REG_GEN_WORD, "cpu.get_reg16({0})"}, _
        {OpCodes.REG_GEN_BYTE, "cpu.get_reg8({0})"}, _
        {OpCodes.REG_SEGMENT, "mem.segment_table[{0}]"}, _
        {OpCodes.REG_XMM, "cpu.xmm[{0}]"}, _
        {OpCodes.REG_FPU, "cpu.get_fpu({0})"} _
    }

    Public OT_a As UInteger = &H1000000
    Public OT_b As UInteger = &H2000000        ' always 1 byte
    Public OT_c As UInteger = &H3000000        ' byte or word, depending on operand
    Public OT_d As UInteger = &H4000000        ' double-word
    Public OT_q As UInteger = &H5000000        ' quad-word
    Public OT_dq As UInteger = &H6000000       ' double quad-word
    Public OT_v As UInteger = &H7000000        ' word or double-word, depending on operand
    Public OT_w As UInteger = &H8000000        ' always word
    Public OT_p As UInteger = &H9000000        ' 32-bit or 48-bit pointer
    Public OT_pi As UInteger = &HA000000       ' quadword MMX register
    Public OT_pd As UInteger = &HB000000       ' 128-bit double-precision float
    Public OT_ps As UInteger = &HC000000       ' 128-bit single-precision float
    Public OT_s As UInteger = &HD000000        ' 6-byte pseudo descriptor
    Public OT_sd As UInteger = &HE000000       ' Scalar of 128-bit double-precision float
    Public OT_ss As UInteger = &HF000000       ' Scalar of 128-bit single-precision float
    Public OT_si As UInteger = &H10000000       ' Doubleword integer register
    Public OT_t As UInteger = &H11000000        ' 80-bit packed FP data

    Public operand_sizes As New Dictionary(Of UInteger, Integer()) From { _
        {0, New Integer() {4, 2}}, _
        {OT_a, New Integer() {4, 2}}, _
        {OT_b, New Integer() {1, 1}}, _
        {OT_c, New Integer() {2, 1}}, _
        {OT_d, New Integer() {4, 4}}, _
        {OT_q, New Integer() {8, 8}}, _
        {OT_dq, New Integer() {16, 16}}, _
        {OT_v, New Integer() {4, 2}}, _
        {OT_w, New Integer() {2, 2}}, _
        {OT_p, New Integer() {6, 4}}, _
        {OT_ps, New Integer() {16, 16}}, _
        {OT_ss, New Integer() {16, 16}}, _
        {OT_sd, New Integer() {16, 16}}, _
        {OT_pd, New Integer() {16, 16}}, _
        {OT_t, New Integer() {8, 8}} _
   }

    Public REG_EAX As UInteger = LibDasm.REGISTER_EAX
    Public REG_AX As UInteger = LibDasm.REGISTER_EAX
    Public REG_AL As UInteger = LibDasm.REGISTER_EAX
    Public REG_ES As UInteger = LibDasm.REGISTER_EAX
    Public REG_ST0 As UInteger = LibDasm.REGISTER_EAX

    Public REG_ECX As UInteger = LibDasm.REGISTER_ECX
    Public REG_CX As UInteger = LibDasm.REGISTER_ECX
    Public REG_CL As UInteger = LibDasm.REGISTER_ECX
    Public REG_CS As UInteger = LibDasm.REGISTER_ECX
    Public REG_ST1 As UInteger = LibDasm.REGISTER_ECX

    Public REG_EDX As UInteger = LibDasm.REGISTER_EDX
    Public REG_DX As UInteger = LibDasm.REGISTER_EDX
    Public REG_DL As UInteger = LibDasm.REGISTER_EDX
    Public REG_SS As UInteger = LibDasm.REGISTER_EDX
    Public REG_ST2 As UInteger = LibDasm.REGISTER_EDX

    Public REG_EBX As UInteger = LibDasm.REGISTER_EBX
    Public REG_BX As UInteger = LibDasm.REGISTER_EBX
    Public REG_BL As UInteger = LibDasm.REGISTER_EBX
    Public REG_DS As UInteger = LibDasm.REGISTER_EBX
    Public REG_ST3 As UInteger = LibDasm.REGISTER_EBX

    Public REG_ESP As UInteger = LibDasm.REGISTER_ESP
    Public REG_SP As UInteger = LibDasm.REGISTER_ESP
    Public REG_AH As UInteger = LibDasm.REGISTER_ESP
    Public REG_FS As UInteger = LibDasm.REGISTER_ESP
    Public REG_ST4 As UInteger = LibDasm.REGISTER_ESP

    Public REG_EBP As UInteger = LibDasm.REGISTER_EBP
    Public REG_BP As UInteger = LibDasm.REGISTER_EBP
    Public REG_CH As UInteger = LibDasm.REGISTER_EBP
    Public REG_GS As UInteger = LibDasm.REGISTER_EBP
    Public REG_ST5 As UInteger = LibDasm.REGISTER_EBP

    Public REG_ESI As UInteger = LibDasm.REGISTER_ESI
    Public REG_SI As UInteger = LibDasm.REGISTER_ESI
    Public REG_DH As UInteger = LibDasm.REGISTER_ESI
    Public REG_ST6 As UInteger = LibDasm.REGISTER_ESI

    Public REG_EDI As UInteger = LibDasm.REGISTER_EDI
    Public REG_DI As UInteger = LibDasm.REGISTER_EDI
    Public REG_BH As UInteger = LibDasm.REGISTER_EDI
    Public REG_ST7 As UInteger = LibDasm.REGISTER_EDI

    Public REG_NOP As UInteger = LibDasm.REGISTER_NOP

    Public AM_A As UInteger = &H10000  ' Direct address with segment prefix
    Public AM_I As UInteger = &H60000  'Immediate data follows
    Public AM_J As UInteger = &H70000  ' Immediate value is relative to EIP
    Public AM_I1 As UInteger = &H200000 ' Immediate byte 1 encoded in instruction

    Public size_names As New Dictionary(Of UInteger, String) From { _
        {16, "dqword"}, _
        {8, "qword"}, _
        {4, "dword"}, _
        {2, "word"}, _
        {1, "byte"} _
    }

    Public size_types As New Dictionary(Of UInteger, String) From { _
        {8, "uint64_t"}, _
        {4, "uint32_t"}, _
        {2, "uint16_t"}, _
        {1, "uint8_t"} _
    }

    Public signed_types As New Dictionary(Of UInteger, String) From { _
        {8, "int64_t"}, _
        {4, "int32_t"}, _
        {2, "int16_t"}, _
        {1, "int8_t"} _
    }

    Public float_types As New Dictionary(Of UInteger, String) From { _
        {16, "pd"}, _
        {8, "sd"}, _
        {4, "ss"} _
    }

    Public EFL_CF As UShort = 1 << 0
    Public EFL_ZF As UShort = 1 << 6
    Public EFL_SF As UShort = 1 << 7
    Public EFL_OF As UShort = 1 << 11

    Public eflags_affected As New Dictionary(Of String, UShort) From { _
        {"comiss", EFL_CF Or EFL_ZF Or EFL_SF Or EFL_OF}, _
        {"comisd", EFL_CF Or EFL_ZF Or EFL_SF Or EFL_OF}
    }

    Public Function get_flag(ByVal value As String) As String
        If value = "DF" Then
            Return "false"
        End If
        Return String.Format("cpu.get_{0}()", value.ToLower())
    End Function

    Public NAME_TO_EFL As New Dictionary(Of String, UShort) From { _
        {"OF", EFL_OF}, _
        {"CF", EFL_CF}, _
        {"ZF", EFL_ZF}, _
        {"SF", EFL_SF}
    }

    Public Function get_cond_eflags(ByVal v As String) As UShort
        Dim flags As UShort = 0

        For Each flag As String In New String() {"OF", "ZF", "SF", "CF"}
            If Not v.Contains(get_flag(flag)) Then
                Continue For
            End If
            flags = flags Or NAME_TO_EFL(flag)
        Next
        Return flags
    End Function

    Public Function format_cond(ByVal v As String) As String
        For Each flag As String In New String() {"OF", "ZF", "SF", "CF"}
            v = v.Replace(flag, get_flag(flag))
        Next
        Return v
    End Function

    Dim COND_NZ As String = format_cond("!ZF")
    Dim COND_NE As String = format_cond("!ZF")
    Dim COND_L As String = format_cond("SF != OF")
    Dim COND_NLE As String = format_cond("!ZF && SF == OF")
    Dim COND_NBE As String = format_cond("!CF && !ZF")
    Dim COND_BE As String = format_cond("CF || ZF")
    Dim COND_NA As String = format_cond("CF || ZF")
    Dim COND_O As String = format_cond("OF")
    Dim COND_Z As String = format_cond("ZF")
    Dim COND_E As String = format_cond("ZF")
    Dim COND_AE As String = format_cond("!CF")
    Dim COND_NC As String = format_cond("!CF")
    Dim COND_A As String = format_cond("!CF && !ZF")
    Dim COND_B As String = format_cond("CF")
    Dim COND_C As String = format_cond("CF")
    Dim COND_G As String = format_cond("!ZF && SF == OF")
    Dim COND_GE As String = format_cond("SF == OF")
    Dim COND_NL As String = format_cond("SF == OF")
    Dim COND_LE As String = format_cond("ZF || SF != OF")
    Dim COND_NG As String = format_cond("ZF || SF != OF")
    Dim COND_S As String = format_cond("SF")
    Dim COND_NS As String = format_cond("!SF")

    Public eflags_used As New Dictionary(Of String, UShort) From { _
        {"adc", EFL_CF},
        {"sbb", EFL_CF},
        {"rcr", EFL_CF},
        {"cmovae", get_cond_eflags(COND_AE)},
        {"cmova", get_cond_eflags(COND_A)},
        {"cmovbe", get_cond_eflags(COND_BE)},
        {"cmovb", get_cond_eflags(COND_B)},
        {"cmovg", get_cond_eflags(COND_G)},
        {"cmovge", get_cond_eflags(COND_GE)},
        {"cmovl", get_cond_eflags(COND_L)},
        {"cmovle", get_cond_eflags(COND_LE)},
        {"cmove", get_cond_eflags(COND_E)},
        {"cmovne", get_cond_eflags(COND_NE)},
        {"cmovs", get_cond_eflags(COND_S)}
    }

    Public Function get_eflags(ByVal value As UShort) As String()
        Dim l As New List(Of String)
        If value And EFL_CF Then
            l.Add("CF")
        End If
        If value And EFL_ZF Then
            l.Add("ZF")
        End If
        If value And EFL_SF Then
            l.Add("SF")
        End If
        If value And EFL_OF Then
            l.Add("OF")
        End If
        Return l.ToArray()
    End Function

    Public label_makers As New Dictionary(Of String, Byte) From { _
        {"jc", 0},
        {"jz", 0},
        {"jnz", 0},
        {"jl", 0},
        {"jnl", 0},
        {"jna", 0},
        {"jns", 0},
        {"js", 0},
        {"jng", 0},
        {"jnc", 0},
        {"ja", 0},
        {"jb", 0},
        {"jg", 0},
        {"jmp", &HEB},
        {"jmp", &HE9}
    }

    Public Function get_mask_ot(ByVal x As UInteger) As UInteger
        Return x And &HFF000000
    End Function

    Public Function get_mask_am(ByVal x As UInteger) As UInteger
        Return x And &HFF0000
    End Function

    Public Function get_register(ByVal reg As UInteger, ByVal reg_type As UInteger) As String
        Dim reg_name As String = reg_table(reg_type)(reg)
        Return String.Format(reg_ref_table(reg_type), reg_name)
    End Function

    Public Function get_gen_register(ByVal reg As UInteger, ByVal size As Integer) As String
        Dim reg_type As UInteger = OpCodes.REG_GEN_DWORD
        If size = 1 Then
            reg_type = OpCodes.REG_GEN_BYTE
        ElseIf size = 2 Then
            reg_type = OpCodes.REG_GEN_WORD
        Else
            reg_type = OpCodes.REG_GEN_DWORD
        End If
        Return get_register(reg, reg_type)
    End Function

    Public Function get_fpu() As String
        Return get_register(REG_ST0, OpCodes.REG_FPU)
    End Function

    Public Function uint32_to_int32(ByVal v As UInteger) As Integer
        Return Convert.ToInt32(v)
    End Function

    Public Function get_memory(ByVal addr As String, ByVal size As UInteger) As String
        Dim func As String = String.Format("mem.read_{0}", size_names(size))
        Return String.Format("{0}({1})", func, addr)
    End Function

    Public Function get_label_name(ByVal address As UInteger) As String
        Return String.Format("loc_{0}", Hex(address))
    End Function

    Public Function prettify_value(ByVal value As UInteger) As String
        Return String.Format("0x{0}", Hex(value))
    End Function

    '!!!!!!!!!!!!!!! CONFIGURATION STUFF !!!!!!!!!!!!!!

    'entry_point = 0x549AD0 - first function to get processed for conversation
    'set it to zero to attempt a auto-detect the main() in the file.
    Public entry_point As UInteger = &H549AD0

    'static_start & start_end (set both to zero, to disable this)
    'these are processed by dwords so they increase by 4 bytes each time.
    Dim static_start As UInteger = &H558414
    Dim static_end As UInteger = &H558548

    'static_ignore(s), could clear this list if you wanted to.
    'initterm initializers
    Dim static_ignore As New List(Of UInteger) From { _
        &H5566C0,
        &H5566D0,
        &H5566E0
    }

    'dynamic_addressses are which functions are forced to processed for converstion
    '0x5183D0 = generator_func
    Public dynamic_addresses As New List(Of UInteger) From { _
        &H4E1C20, _
        &H430130, _
        &H412460, _
        &H549A10, _
        &H5183D0, _
        &H412D60, _
        &H548990
    }

    'Processing stuff thats used by many files
    '------------------------------------------------- ------------
    'Function stack, all the functions that will be processed.
    Public function_stack As New Queue(Of UInteger)
    Public reloc_addresses As New List(Of UInteger)
    Public reloc_values As New List(Of UInteger)
    Public subs As New List(Of UInteger)
    Public [imports] As New Dictionary(Of UInteger, KeyValuePair(Of ImageImport, ImageImportDirectoryEntry))
    Public import_addresses As New Dictionary(Of String, UInteger)
    '--------------------------------------------------------------

    'Map those sub_428580's with actual names if you wish.
    Public function_alias As New Dictionary(Of UInteger, String) From { _
        {&H4C8580, "initialize_manager"},
        {&H4D7FA0, "initialize_seed"},
        {&H5183D0, "generator_func"},
        {&H431880, "setup_func"}
    }

    'function calls that terminate a function
    Public function_enders As New List(Of String) From { _
        {"_Xlength_errorstdYAXPBDZ_imp"} _
    }

    'Create a import_alias database to make those ugly Ordinal names look like actual function names.
    Public import_alias As New Dictionary(Of String, String) From { _
        {"??2@YAPAXI@Z", "new_func"},
        {"ws2_32#115", "WSAStartup"},
        {"??3@YAXPAX@Z", "delete_func"},
        {"??0exception@std@@QAE@ABQBDH@Z", "exception_ctor_noalloc"},
        {"??0exception@std@@QAE@ABQBD@Z", "exception_ctor"},
        {"??0bad_cast@std@@QAE@PBD@Z", "bad_cast_ctor_charptr"},
        {"??_V@YAXPAX@Z", "delete_arr_func"},
        {"??0?$basic_istream@DU?$char_traits@D@std@@@std@@QAE@PAV?$basic_streambuf@DU?$char_traits@D@std@@@1@_N@Z", "basic_istream_char_ctor"},
        {"??0?$basic_streambuf@DU?$char_traits@D@std@@@std@@IAE@XZ", "basic_streambuf_char_ctor"},
        {"?_Init@?$basic_streambuf@DU?$char_traits@D@std@@@std@@IAEXXZ", "basic_streambuf_char__Init_empty"},
        {"?_Fiopen@std@@YAPAU_iobuf@@PBDHH@Z", "_Fiopen"},
        {"?setstate@?$basic_ios@DU?$char_traits@D@std@@@std@@QAEXH_N@Z", "basic_ios_char_setstate_reraise"},
        {"??1?$basic_streambuf@DU?$char_traits@D@std@@@std@@UAE@XZ", "basic_streambuf_char_dtor"},
        {"??1?$basic_istream@DU?$char_traits@D@std@@@std@@UAE@XZ", "basic_istream_char_dtor"},
        {"??1?$basic_ios@DU?$char_traits@D@std@@@std@@UAE@XZ", "basic_ios_char_dtor"},
        {"??5?$basic_istream@DU?$char_traits@D@std@@@std@@QAEAAV01@AAH@Z", "basic_istream_char_read_int"},
        {"??0?$basic_ios@DU?$char_traits@D@std@@@std@@IAE@XZ", "basic_ios_char_ctor"},
        {"??0?$basic_iostream@DU?$char_traits@D@std@@@std@@QAE@PAV?$basic_streambuf@DU?$char_traits@D@std@@@1@@Z", "basic_iostream_char_ctor"},
        {"??6?$basic_ostream@DU?$char_traits@D@std@@@std@@QAEAAV01@H@Z", "basic_ostream_char_print_int"},
        {"?sputn@?$basic_streambuf@DU?$char_traits@D@std@@@std@@QAE_JPBD_J@Z", "basic_streambuf_char_sputn"},
        {"?uncaught_exception@std@@YA_NXZ", "__uncaught_exception"},
        {"?_Osfx@?$basic_ostream@DU?$char_traits@D@std@@@std@@QAEXXZ", "basic_ostream_char__Osfx"},
        {"?setg@?$basic_streambuf@DU?$char_traits@D@std@@@std@@IAEXPAD00@Z", "basic_streambuf_char_setg"},
        {"??1?$basic_iostream@DU?$char_traits@D@std@@@std@@UAE@XZ", "basic_iostream_char_dtor"},
        {"?_Ios_base_dtor@ios_base@std@@CAXPAV12@@Z", "ios_base_Ios_base_dtor"}
    }

    Public Sub GenerateSourceCode(ByVal filename As String)
        Dim custom_code As String
        Dim base_dir As String
        Dim gen_dir As String
        Dim import_dir As String

        Dim mem_writes As String
        Dim sections As New List(Of Section)
        Dim used_imports As New List(Of UInteger)
        Dim custom_subs As New List(Of UInteger)
        Dim cached_names As String
        Dim cpu As CPU
        Dim load_sections As New List(Of Section)
        Dim load_images As String
        Dim sources As New List(Of String)
        Dim big_sources As New List(Of String)
        Dim writer As CodeWriter

        Dim pe As New PEImage(filename)
        Dim image_base As UInteger = pe.PeHeader.OptionalHeader.ImageBase
        Dim code_base As String = image_base + pe.PeHeader.OptionalHeader.BaseOfCode
        Dim data_base As String = image_base + pe.PeHeader.OptionalHeader.BaseOfData
        If entry_point = 0 Then 'not set, this set's the default one
            entry_point = image_base + pe.PeHeader.OptionalHeader.AddressOfEntryPoint
        End If

        PrintLog(String.Format("Image base: 0x{0}", Hex(image_base)), Color.Red)

        base_dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase)
        gen_dir = Path.Combine(base_dir, "gensrc")
        import_dir = Path.Combine(base_dir, "src", "import")

        If Directory.Exists(gen_dir) = False Then
            Directory.CreateDirectory(gen_dir)
        End If

        'Create main file
        writer = New CodeWriter("out.cpp")
        writer.putln("#include ""main.h""")
        writer.putln("#include ""memory.h""")
        writer.putln("#include ""functions.h""")

        custom_code = ""
        'Attempt to load up custom_code imports
        Dim code_path As String
        For Each f As String In Directory.GetFiles(import_dir)
            code_path = Path.Combine(import_dir, f)
            custom_code += File.ReadAllText(code_path)
        Next

        'Load PE sections into lists
        For Each section As SectionHeader In pe.Sections
            Dim new_section As Section = New Section(pe, section)
            sections.Add(new_section)
            If New String() {"rsrc", "text", "reloc"}.Contains(new_section.Name) Then
                Continue For
            End If
            load_sections.Add(new_section)
        Next

        'Image base: 400000
        'imp.address(es) = 
        '5583F4  5583F8  5583FC  558400  5583B8  5583BC  5583C0  5583C8  5583CC  5583D0
        '5583D4  5583D8  5583DC  5583E0  5583E4  5583E8  5583EC  558000  558004  558008
        '55800C  558010  558014  558018  55801C  558020  558024  558028  55802C  558030
        '558034  558038  55803C  558040  558044  558048  55804C  558050  558054  558058
        '55805C  558060  558064  558068  55806C  558070  558074  558078  55807C  558080
        '558084  558088  55808C  558090  558094  558098  55809C  5580A0  5580A4  5580A8
        '5580AC  5580B0  5580B4  5580B8  5580BC  5580C0  5580C4  5580C8  5580CC  5580D0
        '5580D4  5580D8  5580DC  5580E0  5580E4  5580E8  5580EC  5580F0  5580F4  5580F8
        '5580FC  558100  558104  558108  55810C  558110  558114  558118  55811C  558124
        '558128  55812C  558130  558134  558138  55813C  558140  558144  558148  55814C
        '558150  558154  558158  55815C  558160  558164  558168  55816C  558170  558174
        '558178  55817C  558180  558184  558188  55818C  558190  558194  558198  55819C
        '5581A0  5581A4  5581A8  5581AC  5581B0  5581B4  5581B8  5581BC  5581C0  5581C4
        '5581C8  5581CC  5581D0  5581D4  5581D8  5581DC  5581E0  5581E4  5581E8  5581EC
        '5581F0  5581F4  5581F8  5581FC  558200  558204  558208  55820C  558210  558214
        '558218  55821C  558220  558224  558228  55822C  558230  558234  558238  55823C
        '558240  558244  558248  55824C  558250  558254  558258  55825C  558260  558264
        '558268  55826C  558270  558274  558278  55827C  558280  558284  558288  55828C
        '558294  558298  55829C  5582A0  5582A4  5582A8  5582AC  5582B0  5582B4  5582B8
        '5582BC  5582C0  5582C4  5582C8  5582CC  5582D0  5582D4  5582D8  5582DC  5582E0
        '5582E4  5582E8  5582EC  5582F0  5582F4  5582F8  5582FC  558300  558304  558308
        '55830C  558310  558314  558318  55831C  558320  558324  558328  55832C  558330
        '558334  558338  55833C  558340  558344  558348  55834C  558350  558354  558358
        '55835C  558360  558364  558368  55836C  558370  558374  558378  55837C  558380
        '558384  558388  55838C  558390  558394  558398  55839C  5583A0  5583A4  5583A8
        '5583AC  5583B0

        For Each entry As ImageImportDirectoryEntry In pe.ImportDirectoryTable
            For Each imp As ImageImport In entry.ImageImports
                [imports](imp.IATRVA) = New KeyValuePair(Of ImageImport, ImageImportDirectoryEntry)(imp, entry)
                MsgBox(imp.IATRVA)
            Next
        Next

        'Write the include files for the data sections
        For Each Section As Section In load_sections
            writer.putln(String.Format("#include ""{0}_section.h""", Section.Name))
        Next

        writer.putln("")

        'write master sections.h file
        Dim sections_header_writer As New CodeWriter("sections.h")
        sections_header_writer.start_guard("TERRAINGEN_SECTIONS_H")

        Dim section_base As UInteger
        Dim data As Byte()
        Dim extra As UInteger
        For Each section As Section In load_sections
            section_base = image_base + section.VirtualAddress
            data = section.get_data()
            extra = section.Misc_VirtualSize - data.Length
            ReDim Preserve data(data.Length + extra)
            'write each section.h file
            write_data_header(data, String.Format("{0}_section", section.Name), _
                                String.Format("{0}.h", section.Name))
            sections_header_writer.putlnc("extern char {0}[{1}];", section.Name, data.Length)
        Next

        sections_header_writer.close_guard("TERRAINGEN_SECTIONS_H")
        sections_header_writer.close() 'stop writing to this file

        'TODO: SQLITE3 custom_code hack isn't put in yet.

        'Write functions, starting from entry point
        'start at main() if entry_point is set to zero.
        function_stack.Clear()
        function_stack.Enqueue(entry_point) 'first function to process
        'add remaining functions are added here.
        For Each address As UInteger In dynamic_addresses
            If [imports].ContainsKey(address) AndAlso _
                Not import_addresses.ContainsValue(address) Then
                used_imports.Add(address)
            Else
                function_stack.Enqueue(address)
            End If
        Next

        'add initterm initializers to function stack
        writer.put_method("void init_static")
        For addr As UInteger = static_start To static_end Step 4
            If static_ignore.Contains(addr) Then
                Continue For
            End If
            add_function(addr)
            writer.putln("add_ret();")
            writer.putlnc(String.Format("{0}();", get_function_name(addr)))
        Next
        writer.end_brace()

        ' get reloc ready
        reloc_addresses.Clear()
        reloc_values.Clear()

        writer.put_method("void rebase_data")
        'iterate reloc section
        For Each reloc_block As BaseRelocationBlock In pe.RelocationBlocks
            For Each entry As RelocationPageEntry In reloc_block.Entries
                Dim t As BaseRelocationType = entry.Type
                If t = BaseRelocationType.IMAGE_REL_BASED_ABSOLUTE Then
                    Continue For
                End If
                If t <> BaseRelocationType.IMAGE_REL_BASED_HIGHLOW Then
                    PrintLog(String.Format("has unsupported reloctype {0}", t), Color.Aqua)
                    Continue For
                End If
                Dim addr As UInteger = entry.Offset + image_base
                Dim is_text As Boolean = False
                If get_section(addr).section_name = "text" Then
                    is_text = True
                End If
                If New UInteger() {image_base, _
                                   image_base + &H3C, _
                                   image_base + &H18, _
                                   image_base + &H74, _
                                   image_base + &HE8}.Contains(addr) Then
                    'dos headers
                    Continue For
                End If
                'if get_section(dword).section_name = "text" then
                'Continue For
                'End if
                'reloc_values.Add(DWORD)
                reloc_addresses.Add(addr)
                If is_text Then
                    Continue For
                End If
                Dim src As UInteger = get_reloc(DWORD, True)
                Dim dst As UInteger = get_reloc(addr, False)
                writer.putlnc("mem.write_dword({0}, {1});", dst, src)
            Next
        Next
        writer.end_brace()

        'pe is done garbage collect
        pe = Nothing

        'iterate function stack (processing has began!)
        Dim all_subs_objects As New List(Of Subroutine)
        While function_stack.Count > 0
            Dim address As UInteger = function_stack.Dequeue()
            Dim ida As UInteger = get_ida_address(address)
            all_subs_objects = get_subs(address)
            subs.Add(all_subs_objects(0).address_start)
            For Each [sub] As Subroutine In all_subs_objects
                iterate_tree([sub])
            Next
        End While

        'all gathered addresses
        Dim addresses As New List(Of UInteger)
        addresses.AddRange(subs)
        addresses.AddRange(used_imports)
        addresses.AddRange(custom_subs)

        'Create and write a huge list of functions and names to them.
        writer.put_method("void init_function_map")
        Dim name As String
        For Each address As UInteger In addresses
            name = get_function_name(address)
            writer.putln("cpu.add_function(0x{0}, {1});", address, name)
            writer.end_brace()
        Next

        writer.close()

        writer = New CodeWriter("functions.h")
        writer.start_guard("TERRAINGEN_FUNCTIONS_H")
        For Each address As UInteger In addresses
            name = get_function_name(address)
            writer.putln("extern void {0}();", name)
        Next
        writer.close_guard("TERRAINGEN_FUNCTIONS_H")
        writer.close()

        writer = New CodeWriter("stubs.cpp")
        writer.putln("#include <iostream>")
        writer.putln("")
        Dim stubs As New List(Of UInteger)(used_imports)
        stubs.RemoveAll(Function(e) import_addresses.Values.ToList.Exists(Function(f) f = e))
        stubs.RemoveAll(Function(e) subs.Exists(Function(f) f = e))
        Dim stub_names As New List(Of String)
        For Each imp As UInteger In stubs
            name = get_function_name(imp)
            If is_custom(name) OrElse _
                stub_names.Contains(name) Then
                Continue For
            End If
            stub_names.Add(name)
            writer.put_method("void {0}", name)
            Dim import_name As String = get_import_name(imp)
            writer.putln(String.Format("std::cout << ""Stub: {0}"" << std::endl;", import_name))
            writer.end_brace()
            writer.putln("")
        Next
        writer.close()

        writer = New CodeWriter("Routines.cmake", False) 'no license
        writer.putln("set(GEN_SRCS")
        writer.indent()
        For Each f As String In sources
            name = String.Format("${GEN_DIR}/{0}", Path.GetFileName(f))
            writer.putln(name)
        Next
        writer.dedent()
        writer.putln(")")
        writer.putln("")

        writer.putln("set(GEN_BIG_SRCS")
        writer.indent()
        For Each f As String In big_sources
            name = String.Format("${GEN_DIR}/{0}", Path.GetFileName(f))
            writer.putln(name)
        Next
        writer.dedent()
        writer.putln(")")
        writer.close()

        Dim counts = From n In cpu.mnemonic_stats Group n By n Into Group _
                     Select Number = n, Count = Group.Count() _
                     Order By Count Descending
        Dim counter As UInteger = 1
        For Each c In Counts
            PrintLog(String.Format("{0}) Instruction stats: {1} Count: {2}", counter, c.Number, c.Count), Color.Blue)
            counter += 1
        Next
    End Sub

    Public Sub PrintLog(ByVal text As String, ByVal color As Color)
        frmMain.rtbLog.SelectionStart = frmMain.rtbLog.TextLength
        frmMain.rtbLog.SelectionLength = 0

        frmMain.rtbLog.SelectionColor = color
        frmMain.rtbLog.AppendText(text)
        frmMain.rtbLog.SelectionColor = frmMain.rtbLog.ForeColor
    End Sub

    Sub Main()
        Application.Run(frmMain)
        frmMain.Dispose()
    End Sub
End Module

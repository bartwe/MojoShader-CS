#region License
/* MojoShader# - C# Wrapper for MojoShader
*
* Copyright (c) 2014 Ethan Lee.
*
* This software is provided 'as-is', without any express or implied warranty.
* In no event will the authors be held liable for any damages arising from
* the use of this software.
*
* Permission is granted to anyone to use this software for any purpose,
* including commercial applications, and to alter it and redistribute it
* freely, subject to the following restrictions:
*
* 1. The origin of this software must not be misrepresented; you must not
* claim that you wrote the original software. If you use this software in a
* product, an acknowledgment in the product documentation would be
* appreciated but is not required.
*
* 2. Altered source versions must be plainly marked as such, and must not be
* misrepresented as being the original software.
*
* 3. This notice may not be removed or altered from any source distribution.
*
* Ethan "flibitijibibo" Lee <flibitijibibo@flibitijibibo.com>
*
*/
#endregion

#region Using Statements
using System;
using System.Runtime.InteropServices;
#endregion

public class MojoShader
{
	/* FIXME: Ask Ryan about sizeof(int) issues, particualrly for structs.
	 * The places where this will be at its worst is LayoutKind.Explicit
	 * structures. The good news is that this only matters for Windows,
	 * but still.
	 * -flibit
	 */

	private const string nativeLibName = "MojoShader.dll";

	#region Version Interface

	[DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
	public static extern int MOJOSHADER_version();

	/* IntPtr refers to a statically allocated const char* */
	[DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
	public static extern IntPtr MOJOSHADER_changeset();

	#endregion

	#region Custom malloc/free Function Types

	/* data refers to a void* */
	public delegate IntPtr MOJOSHADER_malloc(int bytes, IntPtr data);

	/* ptr refers to a void, data to a void* */
	public delegate IntPtr MOJOSHADER_free(IntPtr ptr, IntPtr data);

	#endregion

	#region Parse Interface

	public enum MOJOSHADER_shaderType
	{
		MOJOSHADER_TYPE_UNKNOWN =	0,
		MOJOSHADER_TYPE_PIXEL =		(1 << 0),
		MOJOSHADER_TYPE_VERTEX =	(1 << 1),
		MOJOSHADER_TYPE_GEOMETRY =	(1 << 2),
		MOJOSHADER_TYPE_ANY =		-1 // 0xFFFFFFFF, ugh
	}

	public enum MOJOSHADER_attributeType
	{
		MOJOSHADER_ATTRIBUTE_UNKNOWN = -1,
		MOJOSHADER_ATTRIBUTE_BYTE,
		MOJOSHADER_ATTRIBUTE_UBYTE,
		MOJOSHADER_ATTRIBUTE_SHORT,
		MOJOSHADER_ATTRIBUTE_USHORT,
		MOJOSHADER_ATTRIBUTE_INT,
		MOJOSHADER_ATTRIBUTE_UINT,
		MOJOSHADER_ATTRIBUTE_FLOAT,
		MOJOSHADER_ATTRIBUTE_DOUBLE,
		MOJOSHADER_ATTRIBUTE_HALF_FLOAT
	}

	public enum MOJOSHADER_uniformType
	{
		MOJOSHADER_UNIFORM_UNKNOWN = -1,
		MOJOSHADER_UNIFORM_FLOAT,
		MOJOSHADER_UNIFORM_INT,
		MOJOSHADER_UNIFORM_BOOL
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct MOJOSHADER_uniform
	{
		public MOJOSHADER_uniformType type;
		public int index;
		public int array_count;
		public int constant;
		public IntPtr name; // const char*
	}

	[StructLayout(LayoutKind.Explicit)]
	public unsafe struct MOJOSHADER_constant
	{
		[FieldOffset(0)]
			public MOJOSHADER_uniformType type;
		[FieldOffset(4)]
			public int index;
		[FieldOffset(8)]
			public fixed float f[4];
		[FieldOffset(8)]
			public fixed int i[4];
		[FieldOffset(8)]
			public int b;
	}

	public enum MOJOSHADER_samplerType
	{
		MOJOSHADER_SAMPLER_UNKNOWN = -1,
		MOJOSHADER_SAMPLER_2D,
		MOJOSHADER_SAMPLER_CUBE,
		MOJOSHADER_SAMPLER_VOLUME
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct MOJOSHADER_sampler
	{
		public MOJOSHADER_samplerType type;
		public int index;
		public IntPtr name; // const char*
		public int texbem;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct MOJOSHADER_samplerMap
	{
		public int index;
		public MOJOSHADER_samplerType type;
	}

	public enum MOJOSHADER_usage
	{
		MOJOSHADER_USAGE_UNKNOWN = -1,
		MOJOSHADER_USAGE_POSITION,
		MOJOSHADER_USAGE_BLENDWEIGHT,
		MOJOSHADER_USAGE_BLENDINDICES,
		MOJOSHADER_USAGE_NORMAL,
		MOJOSHADER_USAGE_POINTSIZE,
		MOJOSHADER_USAGE_TEXCOORD,
		MOJOSHADER_USAGE_TANGENT,
		MOJOSHADER_USAGE_BINORMAL,
		MOJOSHADER_USAGE_TESSFACTOR,
		MOJOSHADER_USAGE_POSITIONT,
		MOJOSHADER_USAGE_COLOR,
		MOJOSHADER_USAGE_FOG,
		MOJOSHADER_USAGE_DEPTH,
		MOJOSHADER_USAGE_SAMPLE,
		MOJOSHADER_USAGE_TOTAL
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct MOJOSHADER_attribute
	{
		public MOJOSHADER_usage usage;
		public int index;
		public IntPtr name; // const char*
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct MOJOSHADER_swizzle
	{
		public MOJOSHADER_usage usage;
		public uint index;
		public fixed byte swizzles[4];
	}

	public enum MOJOSHADER_symbolRegisterSet
	{
		MOJOSHADER_SYMREGSET_BOOL,
		MOJOSHADER_SYMREGSET_INT4,
		MOJOSHADER_SYMREGSET_FLOAT4,
		MOJOSHADER_SYMREGSET_SAMPLER
	}

	public enum MOJOSHADER_symbolClass
	{
		MOJOSHADER_SYMCLASS_SCALAR,
		MOJOSHADER_SYMCLASS_VECTOR,
		MOJOSHADER_SYMCLASS_MATRIX_ROWS,
		MOJOSHADER_SYMCLASS_MATRIX_COLUMNS,
		MOJOSHADER_SYMCLASS_OBJECT,
		MOJOSHADER_SYMCLASS_STRUCT
	}

	public enum MOJOSHADER_symbolType
	{
		MOJOSHADER_SYMTYPE_VOID,
		MOJOSHADER_SYMTYPE_BOOL,
		MOJOSHADER_SYMTYPE_INT,
		MOJOSHADER_SYMTYPE_FLOAT,
		MOJOSHADER_SYMTYPE_STRING,
		MOJOSHADER_SYMTYPE_TEXTURE,
		MOJOSHADER_SYMTYPE_TEXTURE1D,
		MOJOSHADER_SYMTYPE_TEXTURE2D,
		MOJOSHADER_SYMTYPE_TEXTURE3D,
		MOJOSHADER_SYMTYPE_TEXTURECUBE,
		MOJOSHADER_SYMTYPE_SAMPLER,
		MOJOSHADER_SYMTYPE_SAMPLER1D,
		MOJOSHADER_SYMTYPE_SAMPLER2D,
		MOJOSHADER_SYMTYPE_SAMPLER3D,
		MOJOSHADER_SYMTYPE_SAMPLERCUBE,
		MOJOSHADER_SYMTYPE_PIXELSHADER,
		MOJOSHADER_SYMTYPE_VERTEXSHADER,
		MOJOSHADER_SYMTYPE_PIXELFRAGMENT,
		MOJOSHADER_SYMTYPE_VERTEXFRAGMENT,
		MOJOSHADER_SYMTYPE_UNSUPPORTED
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct MOJOSHADER_symbolTypeInfo
	{
		public MOJOSHADER_symbolClass parameter_class;
		public MOJOSHADER_symbolType parameter_type;
		public uint rows;
		public uint columns;
		public uint elements;
		public uint member_count;
		public IntPtr members; // MOJOSHADER_symbolStructMember*
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct MOJOSHADER_symbolStructMember
	{
		public IntPtr name; //const char*
		public MOJOSHADER_symbolTypeInfo info;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct MOJOSHADER_symbol
	{
		public IntPtr name; // const char*
		public MOJOSHADER_symbolRegisterSet register_set;
		public uint register_index;
		public uint register_count;
		public MOJOSHADER_symbolTypeInfo info;
	}

	public const int MOJOSHADER_POSITION_NONE =	-3;
	public const int MOJOSHADER_POSITION_BEFORE =	-2;
	public const int MOJOSHADER_POSITION_AFTER =	-1;
	
	[StructLayout(LayoutKind.Sequential)]
	public struct MOJOSHADER_error
	{
		public IntPtr error; // const char*
		public IntPtr filename; // const char*
		public int error_position;
	}

	public enum MOJOSHADER_preshaderOpcode
	{
		MOJOSHADER_PRESHADEROP_NOP,
		MOJOSHADER_PRESHADEROP_MOV,
		MOJOSHADER_PRESHADEROP_NEG,
		MOJOSHADER_PRESHADEROP_RCP,
		MOJOSHADER_PRESHADEROP_FRC,
		MOJOSHADER_PRESHADEROP_EXP,
		MOJOSHADER_PRESHADEROP_LOG,
		MOJOSHADER_PRESHADEROP_RSQ,
		MOJOSHADER_PRESHADEROP_SIN,
		MOJOSHADER_PRESHADEROP_COS,
		MOJOSHADER_PRESHADEROP_ASIN,
		MOJOSHADER_PRESHADEROP_ACOS,
		MOJOSHADER_PRESHADEROP_ATAN,
		MOJOSHADER_PRESHADEROP_MIN,
		MOJOSHADER_PRESHADEROP_MAX,
		MOJOSHADER_PRESHADEROP_LT,
		MOJOSHADER_PRESHADEROP_GE,
		MOJOSHADER_PRESHADEROP_ADD,
		MOJOSHADER_PRESHADEROP_MUL,
		MOJOSHADER_PRESHADEROP_ATAN2,
		MOJOSHADER_PRESHADEROP_DIV,
		MOJOSHADER_PRESHADEROP_CMP,
		MOJOSHADER_PRESHADEROP_MOVC,
		MOJOSHADER_PRESHADEROP_DOT,
		MOJOSHADER_PRESHADEROP_NOISE,
		MOJOSHADER_PRESHADEROP_SCALAR_OPS,
		MOJOSHADER_PRESHADEROP_MIN_SCALAR = MOJOSHADER_PRESHADEROP_SCALAR_OPS,
		MOJOSHADER_PRESHADEROP_MAX_SCALAR,
		MOJOSHADER_PRESHADEROP_LT_SCALAR,
		MOJOSHADER_PRESHADEROP_GE_SCALAR,
		MOJOSHADER_PRESHADEROP_ADD_SCALAR,
		MOJOSHADER_PRESHADEROP_MUL_SCALAR,
		MOJOSHADER_PRESHADEROP_ATAN2_SCALAR,
		MOJOSHADER_PRESHADEROP_DIV_SCALAR,
		MOJOSHADER_PRESHADEROP_DOT_SCALAR,
		MOJOSHADER_PRESHADEROP_NOISE_SCALAR
	}

	public enum MOJOSHADER_preshaderOperandType
	{
		MOJOSHADER_PRESHADEROPERAND_INPUT,
		MOJOSHADER_PRESHADEROPERAND_OUTPUT,
		MOJOSHADER_PRESHADEROPERAND_LITERAL,
		MOJOSHADER_PRESHADEROPERAND_TEMP
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct MOJOSHADER_preshaderOperand
	{
		public MOJOSHADER_preshaderOperandType type;
		public uint index;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct MOJOSHADER_preshaderInstruction
	{
		public MOJOSHADER_preshaderOpcode opcode;
		public uint element_count;
		public uint operand_count;
		// FIXME: public fixed MOJOSHADER_preshaderOperand operands[3];
		public MOJOSHADER_preshaderOperand operand1;
		public MOJOSHADER_preshaderOperand operand2;
		public MOJOSHADER_preshaderOperand operand3;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct MOJOSHADER_preshader
	{
		public uint literal_count;
		public IntPtr literals; // double*
		public uint temp_count;
		public uint symbol_count;
		public IntPtr symbols; // MOJOSHADER_symbol*
		public uint instruction_count;
		public IntPtr instructions; // MOJOSHADER_preshaderInstruction*
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct MOJOSHADER_parseData
	{
		public int error_count;
		public IntPtr errors; // MOJOSHADER_errors*
		public IntPtr profile; // const char*
		public IntPtr output; // const char*
		public int output_len;
		public int instruction_count;
		public MOJOSHADER_shaderType shader_type;
		public int major_ver;
		public int minor_ver;
		public int uniform_count;
		public IntPtr uniforms; // MOJOSHADER_uniform*
		public int constant_count;
		public int sampler_count;
		public IntPtr samplers; // MOJOSHADER_sampler*
		public int attribute_count;
		public IntPtr attributes; // MOJOSHADER_attribute*
		public int output_count;
		public IntPtr outputs; // MOJOSHADER_attributes*
		public int swizzle_count;
		public IntPtr swizzles; // MOJOSHADER_swizzle*
		public int symbol_count;
		public IntPtr symbols; // MOJOSHADER_symbols*
		public IntPtr preshader; // MOJOSHADER_preshader*
		public MOJOSHADER_malloc malloc;
		public MOJOSHADER_free free;
		public IntPtr malloc_data; // void*
	}

	public const string MOJOSHADER_PROFILE_D3D =		"d3d";
	public const string MOJOSHADER_PROFILE_BYTECODE =	"bytecode";
	public const string MOJOSHADER_PROFILE_GLSL =		"glsl";
	public const string MOJOSHADER_PROFILE_GLSL120 =	"glsl120";
	public const string MOJOSHADER_PROFILE_ARB1 =		"arb1";
	public const string MOJOSHADER_PROFILE_NV2 =		"nv2";
	public const string MOJOSHADER_PROFILE_NV3 =		"nv3";
	public const string MOJOSHADER_PROFILE_NV4 =		"nv4";

	[DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
	public static extern int MOJOSHADER_maxShaderModel(
		[MarshalAs(UnmanagedType.LPStr)]
			string profile
	);

	/* IntPtr refers to a MOJOSHADER_parseData*, d to a void* */
	[DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
	public static extern IntPtr MOJOSHADER_parse(
		[MarshalAs(UnmanagedType.LPStr)]
			string profile,
		byte[] tokenbuf,
		uint bufsize,
		MOJOSHADER_swizzle[] swiz,
		uint swizcount,
		MOJOSHADER_samplerMap[] smap,
		uint smapcount,
		MOJOSHADER_malloc m,
		MOJOSHADER_free f,
		IntPtr d
	);

	/* data refers to a MOJOSHADER_parseData* */
	[DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
	public static extern void MOJOSHADER_freeParseData(IntPtr data);

	#endregion

	#region Effects Interface

	[StructLayout(LayoutKind.Sequential)]
	public struct MOJOSHADER_effectParam
	{
		public IntPtr name; // const char*
		public IntPtr semantic; // const char*
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct MOJOSHADER_effectState
	{
		public int type;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct MOJOSHADER_effectPass
	{
		public IntPtr name; // const char*
		public uint state_count;
		public IntPtr states; // MOJOSHADER_effectState*
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct MOJOSHADER_effectTechnique
	{
		public IntPtr name; // const char*
		public uint pass_count;
		public IntPtr passes; // MOJOSHADER_effectPass*
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct MOJOSHADER_effectTexture
	{
		public uint param;
		public IntPtr name; // const char*
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct MOJOSHADER_effectShader
	{
		public uint technique;
		public uint pass;
		public IntPtr shader; // const MOJOSHADER_parseData*
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct MOJOSHADER_effect
	{
		public int error_count;
		public IntPtr errors; // MOJOSHADER_error*
		public IntPtr profile; // const char*
		public int param_count;
		public IntPtr parameters; // MOJOSHADER_effectParam* params, lolC#
		public int technique_count;
		public IntPtr techniques; // MOJOSHADER_effectTechnique*
		public int texture_count;
		public IntPtr textures; // MOJOSHADER_effectTextures*
		public int shader_count;
		public IntPtr shaders; // MOJOSHADER_effectShader*
		public MOJOSHADER_malloc m;
		public MOJOSHADER_free f;
		public IntPtr malloc_data; // void*
	}

	/* IntPtr refers to a MOJOSHADER_effect*, d to a void* */
	[DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
	public static extern IntPtr MOJOSHADER_parseEffect(
		[MarshalAs(UnmanagedType.LPStr)]
			string profile,
		byte[] buf,
		uint _len,
		MOJOSHADER_swizzle[] swiz,
		uint swizcount,
		MOJOSHADER_samplerMap[] smap,
		uint smapcount,
		MOJOSHADER_malloc m,
		MOJOSHADER_free f,
		IntPtr d
	);

	/* effect refers to a MOJOSHADER_effect* */
	[DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
	public static extern void MOJOSHADER_freeEffect(IntPtr effect);

	#endregion

	#region Preprocessor Interface

	// TODO: Needed for MojoShader#? -flibit

	#endregion

	#region Assembler Interface

	// TODO: Needed for MojoShader#? -flibit

	#endregion

	#region HLSL Support

	// TODO: Needed for MojoShader#? -flibit

	#endregion

	#region Abtract Syntax Tree Interface

	// TODO: Needed for MojoShader#? -flibit

	#endregion

	#region Intermediate Representation Interface

	// TODO: Needed for MojoShader#? -flibit

	#endregion

	#region Compiler Interface

	// TODO: Needed for MojoShader#? -flibit

	#endregion

	#region OpenGL Interface

	public delegate IntPtr MOJOSHADER_glGetProcAddress(
		[MarshalAs(UnmanagedType.LPStr)]
			string fnname,
		IntPtr data
	);

	/* lookup_d refers to a void*, malloc_d to a void* */
	[DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
	public static extern int MOJOSHADER_glAvailableProfiles(
		MOJOSHADER_glGetProcAddress lookup,
		IntPtr lookup_d,
		IntPtr profs, // FIXME: const char**
		int size,
		MOJOSHADER_malloc m,
		MOJOSHADER_free f,
		IntPtr malloc_d
	);

	/* lookup_d refers to a void*, malloc_d to a void* */
	[DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
	private static extern IntPtr INTERNAL_glBestProfile(
		MOJOSHADER_glGetProcAddress lookup,
		IntPtr lookup_d,
		MOJOSHADER_malloc m,
		MOJOSHADER_free f,
		IntPtr malloc_d
	);
	public static string MOJOSHADER_glBestProfile(
		MOJOSHADER_glGetProcAddress lookup,
		IntPtr lookup_d,
		MOJOSHADER_malloc m,
		MOJOSHADER_free f,
		IntPtr malloc_d
	) {
		return Marshal.PtrToStringAnsi(
			INTERNAL_glBestProfile(
				lookup,
				lookup_d,
				m,
				f,
				malloc_d
			)
		);
	}

	/* IntPtr refers to a MOJOSHADER_glContext,
	 * lookup_d to a void*, malloc_d to a void*
	 */
	[DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
	public static extern IntPtr MOJOSHADER_glCreateContext(
		[MarshalAs(UnmanagedType.LPStr)]
			string profile,
		MOJOSHADER_glGetProcAddress lookup,
		IntPtr lookup_d,
		MOJOSHADER_malloc m,
		MOJOSHADER_free f,
		IntPtr malloc_d
	);

	/* ctx refers to a MOJOSHADER_glContext* */
	[DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
	public static extern void MOJOSHADER_glMakeContextCurrent(IntPtr ctx);

	[DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
	private static extern IntPtr INTERNAL_glGetError();
	public static string MOJOSHADER_glGetError()
	{
		return Marshal.PtrToStringAnsi(INTERNAL_glGetError());
	}

	[DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
	public static extern int MOJOSHADER_glMaxUniforms(
		MOJOSHADER_shaderType shader_type
	);

	/* IntPtr refers to a MOJOSHADER_glShader* */
	[DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
	public static extern IntPtr MOJOSHADER_glCompileShader(
		byte[] tokenbuf,
		uint bufsize,
		MOJOSHADER_swizzle[] swiz,
		uint swizcount,
		MOJOSHADER_samplerMap[] smap,
		uint smapcount
	);

	/* IntPtr refers to a const MOJOSHADER_parseData*
	 * shader refers to a MOJOSHADER_glShader*
	 */
	[DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
	public static extern IntPtr MOJOSHADER_glGetShaderParseData(
		IntPtr shader
	);

	/* IntPtr refers to a MOJOSHADER_glProgram*
	 * vshader/pshader refer to a MOJOSHADER_glShader*
	 */
	[DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
	public static extern IntPtr MOJOSHADER_glLinkProgram(
		IntPtr vshader,
		IntPtr pshader
	);

	/* program refers to a MOJOSHADER_glProgram* */
	[DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
	public static extern void MOJOSHADER_glBindProgram(IntPtr program);

	/* vshader/pshader refer to a MOJOSHADER_glShader* */
	[DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
	public static extern void MOJOSHADER_glBindShaders(
		IntPtr vshader,
		IntPtr pshader
	);

	/* data refers to a const float* */
	[DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
	public static extern void MOJOSHADER_glSetVertexShaderUniformF(
		uint idx,
		IntPtr data,
		uint vec4count
	);

	/* data refers to a float* */
	[DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
	public static extern void MOJOSHADER_glGetVertexShaderUniformF(
		uint idx,
		IntPtr data,
		uint vec4count
	);

	/* data refers to a const int* */
	[DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
	public static extern void MOJOSHADER_glSetVertexShaderUniformI(
		uint idx,
		IntPtr data,
		uint ivec4count
	);

	/* data refers to an int* */
	[DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
	public static extern void MOJOSHADER_glGetVertexShaderUniformI(
		uint idx,
		IntPtr data,
		uint ivec4count
	);

	/* data refers to a const int* */
	[DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
	public static extern void MOJOSHADER_glSetVertexShaderUniformB(
		uint idx,
		IntPtr data,
		uint bcount
	);

	/* data refers to an int* */
	[DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
	public static extern void MOJOSHADER_glGetVertexShaderUniformB(
		uint idx,
		IntPtr data,
		uint bcount
	);

	/* data refers to a const float* */
	[DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
	public static extern void MOJOSHADER_glSetPixelShaderUniformF(
		uint idx,
		IntPtr data,
		uint vec4count
	);

	/* data refers to a float* */
	[DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
	public static extern void MOJOSHADER_glGetPixelShaderUniformF(
		uint idx,
		IntPtr data,
		uint vec4count
	);

	/* data refers to a const int* */
	[DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
	public static extern void MOJOSHADER_glSetPixelShaderUniformI(
		uint idx,
		IntPtr data,
		uint ivec4count
	);

	/* data refers to an int* */
	[DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
	public static extern void MOJOSHADER_glGetPixelShaderUniformI(
		uint idx,
		IntPtr data,
		uint ivec4count
	);

	/* data refers to a const int* */
	[DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
	public static extern void MOJOSHADER_glSetPixelShaderUniformB(
		uint idx,
		IntPtr data,
		uint bcount
	);

	/* data refers to an int* */
	[DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
	public static extern void MOJOSHADER_glGetPixelShaderUniformB(
		uint idx,
		IntPtr data,
		uint bcount
	);

	[DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
	public static extern void MOJOSHADER_glSetLegacyBumpMapEnv(
		uint sampler,
		float mat00,
		float mat01,
		float mat10,
		float mat11,
		float lscale,
		float loffset
	);

	/* ptr refers to a const void* */
	[DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
	public static extern void MOJOSHADER_glSetVertexAttribute(
		MOJOSHADER_usage usage,
		int index,
		uint size,
		MOJOSHADER_attributeType type,
		int normalized,
		uint stride,
		IntPtr ptr
	);

	// FIXME: Ryan still needs to merge my patch for this! -flibit
	[DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
	public static extern void MOJOSHADER_glSetVertexAttribDivisor(
		MOJOSHADER_usage usage,
		int index,
		uint divisor
	);

	/* data refers to a const float* */
	[DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
	public static extern void MOJOSHADER_glSetVertexPreshaderUniformF(
		uint idx,
		IntPtr data,
		uint vec4n
	);

	/* data refers to a float* */
	[DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
	public static extern void MOJOSHADER_glGetVertexPreshaderUniformF(
		uint idx,
		IntPtr data,
		uint vec4n
	);

	/* data refers to a const float* */
	[DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
	public static extern void MOJOSHADER_glSetPixelPreshaderUniformF(
		uint idx,
		IntPtr data,
		uint vec4n
	);

	/* data refers to a float* */
	[DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
	public static extern void MOJOSHADER_glGetPixelPreshaderUniformF(
		uint idx,
		IntPtr data,
		uint vec4n
	);

	[DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
	public static extern void MOJOSHADER_glProgramReady();

	/* program refers to a MOJOSHADER_glProgram* */
	[DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
	public static extern void MOJOSHADER_glDeleteProgram(IntPtr program);

	/* shader refers to a MOJOSHADER_glShader* */
	[DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
	public static extern void MOJOSHADER_glDeleteShader(IntPtr shader);

	/* ctx refers to a MOJOSHADER_glContext* */
	[DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
	public static extern void MOJOSHADER_glDestroyContext(IntPtr ctx);

	#endregion
}

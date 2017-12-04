﻿using System;
using OpenTK.Graphics.OpenGL4;

namespace GRaff.Graphics.Shaders
{
    public class UniformTexture : UniformVariable
    {
        public UniformTexture(ShaderProgram program, string name)
            : base(program, name)
        { }

        public UniformTexture(ShaderProgram program, string name, int index)
            : base(program, name)
        {
            this.Index = index;
        }

        public int Index
        {
            get
            {
                Verify();
                GL.GetUniform(Program.Id, Location, out int value);
                return value;
            }

            set
            {
                Verify();
                Contract.Requires<ArgumentOutOfRangeException>(value >= 0 || value < 32, nameof(value));
                GL.ProgramUniform1(Program.Id, Location, value);
            }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Numerics;
using System.Linq;
using GlmSharp.Swizzle;

// ReSharper disable InconsistentNaming

namespace GlmSharp
{
    
    /// <summary>
    /// A matrix of type Half with 3 columns and 2 rows.
    /// </summary>
    [Serializable]
    [DataContract(Namespace = "mat")]
    [StructLayout(LayoutKind.Sequential)]
    public struct hmat3x2 : IReadOnlyList<Half>, IEquatable<hmat3x2>
    {

        #region Fields
        
        /// <summary>
        /// Column 0, Rows 0
        /// </summary>
        [DataMember]
        public Half m00;
        
        /// <summary>
        /// Column 0, Rows 1
        /// </summary>
        [DataMember]
        public Half m01;
        
        /// <summary>
        /// Column 1, Rows 0
        /// </summary>
        [DataMember]
        public Half m10;
        
        /// <summary>
        /// Column 1, Rows 1
        /// </summary>
        [DataMember]
        public Half m11;
        
        /// <summary>
        /// Column 2, Rows 0
        /// </summary>
        [DataMember]
        public Half m20;
        
        /// <summary>
        /// Column 2, Rows 1
        /// </summary>
        [DataMember]
        public Half m21;

        #endregion


        #region Constructors
        
        /// <summary>
        /// Component-wise constructor
        /// </summary>
        public hmat3x2(Half m00, Half m01, Half m10, Half m11, Half m20, Half m21)
        {
            this.m00 = m00;
            this.m01 = m01;
            this.m10 = m10;
            this.m11 = m11;
            this.m20 = m20;
            this.m21 = m21;
        }
        
        /// <summary>
        /// Constructs this matrix from a hmat2. Non-overwritten fields are from an Identity matrix.
        /// </summary>
        public hmat3x2(hmat2 m)
        {
            this.m00 = m.m00;
            this.m01 = m.m01;
            this.m10 = m.m10;
            this.m11 = m.m11;
            this.m20 = Half.Zero;
            this.m21 = Half.Zero;
        }
        
        /// <summary>
        /// Constructs this matrix from a hmat3x2. Non-overwritten fields are from an Identity matrix.
        /// </summary>
        public hmat3x2(hmat3x2 m)
        {
            this.m00 = m.m00;
            this.m01 = m.m01;
            this.m10 = m.m10;
            this.m11 = m.m11;
            this.m20 = m.m20;
            this.m21 = m.m21;
        }
        
        /// <summary>
        /// Constructs this matrix from a hmat4x2. Non-overwritten fields are from an Identity matrix.
        /// </summary>
        public hmat3x2(hmat4x2 m)
        {
            this.m00 = m.m00;
            this.m01 = m.m01;
            this.m10 = m.m10;
            this.m11 = m.m11;
            this.m20 = m.m20;
            this.m21 = m.m21;
        }
        
        /// <summary>
        /// Constructs this matrix from a hmat2x3. Non-overwritten fields are from an Identity matrix.
        /// </summary>
        public hmat3x2(hmat2x3 m)
        {
            this.m00 = m.m00;
            this.m01 = m.m01;
            this.m10 = m.m10;
            this.m11 = m.m11;
            this.m20 = Half.Zero;
            this.m21 = Half.Zero;
        }
        
        /// <summary>
        /// Constructs this matrix from a hmat3. Non-overwritten fields are from an Identity matrix.
        /// </summary>
        public hmat3x2(hmat3 m)
        {
            this.m00 = m.m00;
            this.m01 = m.m01;
            this.m10 = m.m10;
            this.m11 = m.m11;
            this.m20 = m.m20;
            this.m21 = m.m21;
        }
        
        /// <summary>
        /// Constructs this matrix from a hmat4x3. Non-overwritten fields are from an Identity matrix.
        /// </summary>
        public hmat3x2(hmat4x3 m)
        {
            this.m00 = m.m00;
            this.m01 = m.m01;
            this.m10 = m.m10;
            this.m11 = m.m11;
            this.m20 = m.m20;
            this.m21 = m.m21;
        }
        
        /// <summary>
        /// Constructs this matrix from a hmat2x4. Non-overwritten fields are from an Identity matrix.
        /// </summary>
        public hmat3x2(hmat2x4 m)
        {
            this.m00 = m.m00;
            this.m01 = m.m01;
            this.m10 = m.m10;
            this.m11 = m.m11;
            this.m20 = Half.Zero;
            this.m21 = Half.Zero;
        }
        
        /// <summary>
        /// Constructs this matrix from a hmat3x4. Non-overwritten fields are from an Identity matrix.
        /// </summary>
        public hmat3x2(hmat3x4 m)
        {
            this.m00 = m.m00;
            this.m01 = m.m01;
            this.m10 = m.m10;
            this.m11 = m.m11;
            this.m20 = m.m20;
            this.m21 = m.m21;
        }
        
        /// <summary>
        /// Constructs this matrix from a hmat4. Non-overwritten fields are from an Identity matrix.
        /// </summary>
        public hmat3x2(hmat4 m)
        {
            this.m00 = m.m00;
            this.m01 = m.m01;
            this.m10 = m.m10;
            this.m11 = m.m11;
            this.m20 = m.m20;
            this.m21 = m.m21;
        }
        
        /// <summary>
        /// Constructs this matrix from a series of column vectors. Non-overwritten fields are from an Identity matrix.
        /// </summary>
        public hmat3x2(hvec2 c0, hvec2 c1)
        {
            this.m00 = c0.x;
            this.m01 = c0.y;
            this.m10 = c1.x;
            this.m11 = c1.y;
            this.m20 = Half.Zero;
            this.m21 = Half.Zero;
        }
        
        /// <summary>
        /// Constructs this matrix from a series of column vectors. Non-overwritten fields are from an Identity matrix.
        /// </summary>
        public hmat3x2(hvec2 c0, hvec2 c1, hvec2 c2)
        {
            this.m00 = c0.x;
            this.m01 = c0.y;
            this.m10 = c1.x;
            this.m11 = c1.y;
            this.m20 = c2.x;
            this.m21 = c2.y;
        }

        #endregion


        #region Properties
        
        /// <summary>
        /// Creates a 2D array with all values (address: Values[x, y])
        /// </summary>
        public Half[,] Values => new[,] { { m00, m01 }, { m10, m11 }, { m20, m21 } };
        
        /// <summary>
        /// Creates a 1D array with all values (internal order)
        /// </summary>
        public Half[] Values1D => new[] { m00, m01, m10, m11, m20, m21 };
        
        /// <summary>
        /// Gets or sets the column nr 0
        /// </summary>
        public hvec2 Column0
        {
            get
            {
                return new hvec2(m00, m01);
            }
            set
            {
                m00 = value.x;
                m01 = value.y;
            }
        }
        
        /// <summary>
        /// Gets or sets the column nr 1
        /// </summary>
        public hvec2 Column1
        {
            get
            {
                return new hvec2(m10, m11);
            }
            set
            {
                m10 = value.x;
                m11 = value.y;
            }
        }
        
        /// <summary>
        /// Gets or sets the column nr 2
        /// </summary>
        public hvec2 Column2
        {
            get
            {
                return new hvec2(m20, m21);
            }
            set
            {
                m20 = value.x;
                m21 = value.y;
            }
        }
        
        /// <summary>
        /// Gets or sets the row nr 0
        /// </summary>
        public hvec3 Row0
        {
            get
            {
                return new hvec3(m00, m10, m20);
            }
            set
            {
                m00 = value.x;
                m10 = value.y;
                m20 = value.z;
            }
        }
        
        /// <summary>
        /// Gets or sets the row nr 1
        /// </summary>
        public hvec3 Row1
        {
            get
            {
                return new hvec3(m01, m11, m21);
            }
            set
            {
                m01 = value.x;
                m11 = value.y;
                m21 = value.z;
            }
        }

        #endregion


        #region Static Properties
        
        /// <summary>
        /// Predefined all-zero matrix
        /// </summary>
        public static hmat3x2 Zero { get; } = new hmat3x2(Half.Zero, Half.Zero, Half.Zero, Half.Zero, Half.Zero, Half.Zero);
        
        /// <summary>
        /// Predefined all-ones matrix
        /// </summary>
        public static hmat3x2 Ones { get; } = new hmat3x2(Half.One, Half.One, Half.One, Half.One, Half.One, Half.One);
        
        /// <summary>
        /// Predefined identity matrix
        /// </summary>
        public static hmat3x2 Identity { get; } = new hmat3x2(Half.One, Half.Zero, Half.Zero, Half.One, Half.Zero, Half.Zero);
        
        /// <summary>
        /// Predefined all-MaxValue matrix
        /// </summary>
        public static hmat3x2 AllMaxValue { get; } = new hmat3x2(Half.MaxValue, Half.MaxValue, Half.MaxValue, Half.MaxValue, Half.MaxValue, Half.MaxValue);
        
        /// <summary>
        /// Predefined diagonal-MaxValue matrix
        /// </summary>
        public static hmat3x2 DiagonalMaxValue { get; } = new hmat3x2(Half.MaxValue, Half.Zero, Half.Zero, Half.MaxValue, Half.Zero, Half.Zero);
        
        /// <summary>
        /// Predefined all-MinValue matrix
        /// </summary>
        public static hmat3x2 AllMinValue { get; } = new hmat3x2(Half.MinValue, Half.MinValue, Half.MinValue, Half.MinValue, Half.MinValue, Half.MinValue);
        
        /// <summary>
        /// Predefined diagonal-MinValue matrix
        /// </summary>
        public static hmat3x2 DiagonalMinValue { get; } = new hmat3x2(Half.MinValue, Half.Zero, Half.Zero, Half.MinValue, Half.Zero, Half.Zero);
        
        /// <summary>
        /// Predefined all-Epsilon matrix
        /// </summary>
        public static hmat3x2 AllEpsilon { get; } = new hmat3x2(Half.Epsilon, Half.Epsilon, Half.Epsilon, Half.Epsilon, Half.Epsilon, Half.Epsilon);
        
        /// <summary>
        /// Predefined diagonal-Epsilon matrix
        /// </summary>
        public static hmat3x2 DiagonalEpsilon { get; } = new hmat3x2(Half.Epsilon, Half.Zero, Half.Zero, Half.Epsilon, Half.Zero, Half.Zero);
        
        /// <summary>
        /// Predefined all-NaN matrix
        /// </summary>
        public static hmat3x2 AllNaN { get; } = new hmat3x2(Half.NaN, Half.NaN, Half.NaN, Half.NaN, Half.NaN, Half.NaN);
        
        /// <summary>
        /// Predefined diagonal-NaN matrix
        /// </summary>
        public static hmat3x2 DiagonalNaN { get; } = new hmat3x2(Half.NaN, Half.Zero, Half.Zero, Half.NaN, Half.Zero, Half.Zero);
        
        /// <summary>
        /// Predefined all-NegativeInfinity matrix
        /// </summary>
        public static hmat3x2 AllNegativeInfinity { get; } = new hmat3x2(Half.NegativeInfinity, Half.NegativeInfinity, Half.NegativeInfinity, Half.NegativeInfinity, Half.NegativeInfinity, Half.NegativeInfinity);
        
        /// <summary>
        /// Predefined diagonal-NegativeInfinity matrix
        /// </summary>
        public static hmat3x2 DiagonalNegativeInfinity { get; } = new hmat3x2(Half.NegativeInfinity, Half.Zero, Half.Zero, Half.NegativeInfinity, Half.Zero, Half.Zero);
        
        /// <summary>
        /// Predefined all-PositiveInfinity matrix
        /// </summary>
        public static hmat3x2 AllPositiveInfinity { get; } = new hmat3x2(Half.PositiveInfinity, Half.PositiveInfinity, Half.PositiveInfinity, Half.PositiveInfinity, Half.PositiveInfinity, Half.PositiveInfinity);
        
        /// <summary>
        /// Predefined diagonal-PositiveInfinity matrix
        /// </summary>
        public static hmat3x2 DiagonalPositiveInfinity { get; } = new hmat3x2(Half.PositiveInfinity, Half.Zero, Half.Zero, Half.PositiveInfinity, Half.Zero, Half.Zero);

        #endregion


        #region Functions
        
        /// <summary>
        /// Returns an enumerator that iterates through all fields.
        /// </summary>
        public IEnumerator<Half> GetEnumerator()
        {
            yield return m00;
            yield return m01;
            yield return m10;
            yield return m11;
            yield return m20;
            yield return m21;
        }
        
        /// <summary>
        /// Returns an enumerator that iterates through all fields.
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion

        
        /// <summary>
        /// Returns the number of Fields (3 x 2 = 6).
        /// </summary>
        public int Count => 6;
        
        /// <summary>
        /// Gets/Sets a specific indexed component (a bit slower than direct access).
        /// </summary>
        public Half this[int fieldIndex]
        {
            get
            {
                switch (fieldIndex)
                {
                    case 0: return m00;
                    case 1: return m01;
                    case 2: return m10;
                    case 3: return m11;
                    case 4: return m20;
                    case 5: return m21;
                    default: throw new ArgumentOutOfRangeException("fieldIndex");
                }
            }
            set
            {
                switch (fieldIndex)
                {
                    case 0: this.m00 = value; break;
                    case 1: this.m01 = value; break;
                    case 2: this.m10 = value; break;
                    case 3: this.m11 = value; break;
                    case 4: this.m20 = value; break;
                    case 5: this.m21 = value; break;
                    default: throw new ArgumentOutOfRangeException("fieldIndex");
                }
            }
        }
        
        /// <summary>
        /// Gets/Sets a specific 2D-indexed component (a bit slower than direct access).
        /// </summary>
        public Half this[int col, int row]
        {
            get
            {
                return this[col * 2 + row];
            }
            set
            {
                this[col * 2 + row] = value;
            }
        }
        
        /// <summary>
        /// Returns true iff this equals rhs component-wise.
        /// </summary>
        public bool Equals(hmat3x2 rhs) => (((m00.Equals(rhs.m00) && m01.Equals(rhs.m01)) && m10.Equals(rhs.m10)) && ((m11.Equals(rhs.m11) && m20.Equals(rhs.m20)) && m21.Equals(rhs.m21)));
        
        /// <summary>
        /// Returns true iff this equals rhs type- and component-wise.
        /// </summary>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is hmat3x2 && Equals((hmat3x2) obj);
        }
        
        /// <summary>
        /// Returns true iff this equals rhs component-wise.
        /// </summary>
        public static bool operator ==(hmat3x2 lhs, hmat3x2 rhs) => lhs.Equals(rhs);
        
        /// <summary>
        /// Returns true iff this does not equal rhs (component-wise).
        /// </summary>
        public static bool operator !=(hmat3x2 lhs, hmat3x2 rhs) => !lhs.Equals(rhs);
        
        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        public override int GetHashCode()
        {
            unchecked
            {
                return ((((((((((m00.GetHashCode()) * 397) ^ m01.GetHashCode()) * 397) ^ m10.GetHashCode()) * 397) ^ m11.GetHashCode()) * 397) ^ m20.GetHashCode()) * 397) ^ m21.GetHashCode();
            }
        }
        
        /// <summary>
        /// Returns a transposed version of this matrix.
        /// </summary>
        public hmat2x3 Transposed => new hmat2x3(m00, m10, m20, m01, m11, m21);
        
        /// <summary>
        /// Returns the minimal component of this matrix.
        /// </summary>
        public Half MinElement => Half.Min(Half.Min(Half.Min(Half.Min(Half.Min(m00, m01), m10), m11), m20), m21);
        
        /// <summary>
        /// Returns the maximal component of this matrix.
        /// </summary>
        public Half MaxElement => Half.Max(Half.Max(Half.Max(Half.Max(Half.Max(m00, m01), m10), m11), m20), m21);
        
        /// <summary>
        /// Returns the euclidean length of this matrix.
        /// </summary>
        public float Length => (float)Math.Sqrt((((m00*m00 + m01*m01) + m10*m10) + ((m11*m11 + m20*m20) + m21*m21)));
        
        /// <summary>
        /// Returns the squared euclidean length of this matrix.
        /// </summary>
        public float LengthSqr => (((m00*m00 + m01*m01) + m10*m10) + ((m11*m11 + m20*m20) + m21*m21));
        
        /// <summary>
        /// Returns the sum of all fields.
        /// </summary>
        public Half Sum => (((m00 + m01) + m10) + ((m11 + m20) + m21));
        
        /// <summary>
        /// Returns the euclidean norm of this matrix.
        /// </summary>
        public float Norm => (float)Math.Sqrt((((m00*m00 + m01*m01) + m10*m10) + ((m11*m11 + m20*m20) + m21*m21)));
        
        /// <summary>
        /// Returns the one-norm of this matrix.
        /// </summary>
        public float Norm1 => (((Half.Abs(m00) + Half.Abs(m01)) + Half.Abs(m10)) + ((Half.Abs(m11) + Half.Abs(m20)) + Half.Abs(m21)));
        
        /// <summary>
        /// Returns the two-norm of this matrix.
        /// </summary>
        public float Norm2 => (float)Math.Sqrt((((m00*m00 + m01*m01) + m10*m10) + ((m11*m11 + m20*m20) + m21*m21)));
        
        /// <summary>
        /// Returns the max-norm of this matrix.
        /// </summary>
        public Half NormMax => Half.Max(Half.Max(Half.Max(Half.Max(Half.Max(Half.Abs(m00), Half.Abs(m01)), Half.Abs(m10)), Half.Abs(m11)), Half.Abs(m20)), Half.Abs(m21));
        
        /// <summary>
        /// Returns the p-norm of this matrix.
        /// </summary>
        public double NormP(double p) => Math.Pow((((Math.Pow((double)Half.Abs(m00), p) + Math.Pow((double)Half.Abs(m01), p)) + Math.Pow((double)Half.Abs(m10), p)) + ((Math.Pow((double)Half.Abs(m11), p) + Math.Pow((double)Half.Abs(m20), p)) + Math.Pow((double)Half.Abs(m21), p))), 1 / p);
        
        /// <summary>
        /// Executes a matrix-matrix-multiplication hmat3x2 * hmat2x3 -> hmat2.
        /// </summary>
        public static hmat2 operator*(hmat3x2 lhs, hmat2x3 rhs) => new hmat2(((lhs.m00 * rhs.m00 + lhs.m10 * rhs.m01) + lhs.m20 * rhs.m02), ((lhs.m01 * rhs.m00 + lhs.m11 * rhs.m01) + lhs.m21 * rhs.m02), ((lhs.m00 * rhs.m10 + lhs.m10 * rhs.m11) + lhs.m20 * rhs.m12), ((lhs.m01 * rhs.m10 + lhs.m11 * rhs.m11) + lhs.m21 * rhs.m12));
        
        /// <summary>
        /// Executes a matrix-matrix-multiplication hmat3x2 * hmat3 -> hmat3x2.
        /// </summary>
        public static hmat3x2 operator*(hmat3x2 lhs, hmat3 rhs) => new hmat3x2(((lhs.m00 * rhs.m00 + lhs.m10 * rhs.m01) + lhs.m20 * rhs.m02), ((lhs.m01 * rhs.m00 + lhs.m11 * rhs.m01) + lhs.m21 * rhs.m02), ((lhs.m00 * rhs.m10 + lhs.m10 * rhs.m11) + lhs.m20 * rhs.m12), ((lhs.m01 * rhs.m10 + lhs.m11 * rhs.m11) + lhs.m21 * rhs.m12), ((lhs.m00 * rhs.m20 + lhs.m10 * rhs.m21) + lhs.m20 * rhs.m22), ((lhs.m01 * rhs.m20 + lhs.m11 * rhs.m21) + lhs.m21 * rhs.m22));
        
        /// <summary>
        /// Executes a matrix-matrix-multiplication hmat3x2 * hmat4x3 -> hmat4x2.
        /// </summary>
        public static hmat4x2 operator*(hmat3x2 lhs, hmat4x3 rhs) => new hmat4x2(((lhs.m00 * rhs.m00 + lhs.m10 * rhs.m01) + lhs.m20 * rhs.m02), ((lhs.m01 * rhs.m00 + lhs.m11 * rhs.m01) + lhs.m21 * rhs.m02), ((lhs.m00 * rhs.m10 + lhs.m10 * rhs.m11) + lhs.m20 * rhs.m12), ((lhs.m01 * rhs.m10 + lhs.m11 * rhs.m11) + lhs.m21 * rhs.m12), ((lhs.m00 * rhs.m20 + lhs.m10 * rhs.m21) + lhs.m20 * rhs.m22), ((lhs.m01 * rhs.m20 + lhs.m11 * rhs.m21) + lhs.m21 * rhs.m22), ((lhs.m00 * rhs.m30 + lhs.m10 * rhs.m31) + lhs.m20 * rhs.m32), ((lhs.m01 * rhs.m30 + lhs.m11 * rhs.m31) + lhs.m21 * rhs.m32));
        
        /// <summary>
        /// Executes a matrix-vector-multiplication.
        /// </summary>
        public static hvec2 operator*(hmat3x2 m, hvec3 v) => new hvec2(((m.m00 * v.x + m.m10 * v.y) + m.m20 * v.z), ((m.m01 * v.x + m.m11 * v.y) + m.m21 * v.z));
        
        /// <summary>
        /// Executes a component-wise * (multiply).
        /// </summary>
        public static hmat3x2 CompMul(hmat3x2 A, hmat3x2 B) => new hmat3x2(A.m00 * B.m00, A.m01 * B.m01, A.m10 * B.m10, A.m11 * B.m11, A.m20 * B.m20, A.m21 * B.m21);
        
        /// <summary>
        /// Executes a component-wise / (divide).
        /// </summary>
        public static hmat3x2 CompDiv(hmat3x2 A, hmat3x2 B) => new hmat3x2(A.m00 / B.m00, A.m01 / B.m01, A.m10 / B.m10, A.m11 / B.m11, A.m20 / B.m20, A.m21 / B.m21);
        
        /// <summary>
        /// Executes a component-wise + (add).
        /// </summary>
        public static hmat3x2 CompAdd(hmat3x2 A, hmat3x2 B) => new hmat3x2(A.m00 + B.m00, A.m01 + B.m01, A.m10 + B.m10, A.m11 + B.m11, A.m20 + B.m20, A.m21 + B.m21);
        
        /// <summary>
        /// Executes a component-wise - (subtract).
        /// </summary>
        public static hmat3x2 CompSub(hmat3x2 A, hmat3x2 B) => new hmat3x2(A.m00 - B.m00, A.m01 - B.m01, A.m10 - B.m10, A.m11 - B.m11, A.m20 - B.m20, A.m21 - B.m21);
        
        /// <summary>
        /// Executes a component-wise + (add).
        /// </summary>
        public static hmat3x2 operator+(hmat3x2 lhs, hmat3x2 rhs) => new hmat3x2(lhs.m00 + rhs.m00, lhs.m01 + rhs.m01, lhs.m10 + rhs.m10, lhs.m11 + rhs.m11, lhs.m20 + rhs.m20, lhs.m21 + rhs.m21);
        
        /// <summary>
        /// Executes a component-wise + (add) with a scalar.
        /// </summary>
        public static hmat3x2 operator+(hmat3x2 lhs, Half rhs) => new hmat3x2(lhs.m00 + rhs, lhs.m01 + rhs, lhs.m10 + rhs, lhs.m11 + rhs, lhs.m20 + rhs, lhs.m21 + rhs);
        
        /// <summary>
        /// Executes a component-wise + (add) with a scalar.
        /// </summary>
        public static hmat3x2 operator+(Half lhs, hmat3x2 rhs) => new hmat3x2(lhs + rhs.m00, lhs + rhs.m01, lhs + rhs.m10, lhs + rhs.m11, lhs + rhs.m20, lhs + rhs.m21);
        
        /// <summary>
        /// Executes a component-wise - (subtract).
        /// </summary>
        public static hmat3x2 operator-(hmat3x2 lhs, hmat3x2 rhs) => new hmat3x2(lhs.m00 - rhs.m00, lhs.m01 - rhs.m01, lhs.m10 - rhs.m10, lhs.m11 - rhs.m11, lhs.m20 - rhs.m20, lhs.m21 - rhs.m21);
        
        /// <summary>
        /// Executes a component-wise - (subtract) with a scalar.
        /// </summary>
        public static hmat3x2 operator-(hmat3x2 lhs, Half rhs) => new hmat3x2(lhs.m00 - rhs, lhs.m01 - rhs, lhs.m10 - rhs, lhs.m11 - rhs, lhs.m20 - rhs, lhs.m21 - rhs);
        
        /// <summary>
        /// Executes a component-wise - (subtract) with a scalar.
        /// </summary>
        public static hmat3x2 operator-(Half lhs, hmat3x2 rhs) => new hmat3x2(lhs - rhs.m00, lhs - rhs.m01, lhs - rhs.m10, lhs - rhs.m11, lhs - rhs.m20, lhs - rhs.m21);
        
        /// <summary>
        /// Executes a component-wise / (divide) with a scalar.
        /// </summary>
        public static hmat3x2 operator/(hmat3x2 lhs, Half rhs) => new hmat3x2(lhs.m00 / rhs, lhs.m01 / rhs, lhs.m10 / rhs, lhs.m11 / rhs, lhs.m20 / rhs, lhs.m21 / rhs);
        
        /// <summary>
        /// Executes a component-wise / (divide) with a scalar.
        /// </summary>
        public static hmat3x2 operator/(Half lhs, hmat3x2 rhs) => new hmat3x2(lhs / rhs.m00, lhs / rhs.m01, lhs / rhs.m10, lhs / rhs.m11, lhs / rhs.m20, lhs / rhs.m21);
        
        /// <summary>
        /// Executes a component-wise * (multiply) with a scalar.
        /// </summary>
        public static hmat3x2 operator*(hmat3x2 lhs, Half rhs) => new hmat3x2(lhs.m00 * rhs, lhs.m01 * rhs, lhs.m10 * rhs, lhs.m11 * rhs, lhs.m20 * rhs, lhs.m21 * rhs);
        
        /// <summary>
        /// Executes a component-wise * (multiply) with a scalar.
        /// </summary>
        public static hmat3x2 operator*(Half lhs, hmat3x2 rhs) => new hmat3x2(lhs * rhs.m00, lhs * rhs.m01, lhs * rhs.m10, lhs * rhs.m11, lhs * rhs.m20, lhs * rhs.m21);
        
        /// <summary>
        /// Executes a component-wise lesser-than comparison.
        /// </summary>
        public static bmat3x2 operator<(hmat3x2 lhs, hmat3x2 rhs) => new bmat3x2(lhs.m00 < rhs.m00, lhs.m01 < rhs.m01, lhs.m10 < rhs.m10, lhs.m11 < rhs.m11, lhs.m20 < rhs.m20, lhs.m21 < rhs.m21);
        
        /// <summary>
        /// Executes a component-wise lesser-than comparison with a scalar.
        /// </summary>
        public static bmat3x2 operator<(hmat3x2 lhs, Half rhs) => new bmat3x2(lhs.m00 < rhs, lhs.m01 < rhs, lhs.m10 < rhs, lhs.m11 < rhs, lhs.m20 < rhs, lhs.m21 < rhs);
        
        /// <summary>
        /// Executes a component-wise lesser-than comparison with a scalar.
        /// </summary>
        public static bmat3x2 operator<(Half lhs, hmat3x2 rhs) => new bmat3x2(lhs < rhs.m00, lhs < rhs.m01, lhs < rhs.m10, lhs < rhs.m11, lhs < rhs.m20, lhs < rhs.m21);
        
        /// <summary>
        /// Executes a component-wise lesser-or-equal comparison.
        /// </summary>
        public static bmat3x2 operator<=(hmat3x2 lhs, hmat3x2 rhs) => new bmat3x2(lhs.m00 <= rhs.m00, lhs.m01 <= rhs.m01, lhs.m10 <= rhs.m10, lhs.m11 <= rhs.m11, lhs.m20 <= rhs.m20, lhs.m21 <= rhs.m21);
        
        /// <summary>
        /// Executes a component-wise lesser-or-equal comparison with a scalar.
        /// </summary>
        public static bmat3x2 operator<=(hmat3x2 lhs, Half rhs) => new bmat3x2(lhs.m00 <= rhs, lhs.m01 <= rhs, lhs.m10 <= rhs, lhs.m11 <= rhs, lhs.m20 <= rhs, lhs.m21 <= rhs);
        
        /// <summary>
        /// Executes a component-wise lesser-or-equal comparison with a scalar.
        /// </summary>
        public static bmat3x2 operator<=(Half lhs, hmat3x2 rhs) => new bmat3x2(lhs <= rhs.m00, lhs <= rhs.m01, lhs <= rhs.m10, lhs <= rhs.m11, lhs <= rhs.m20, lhs <= rhs.m21);
        
        /// <summary>
        /// Executes a component-wise greater-than comparison.
        /// </summary>
        public static bmat3x2 operator>(hmat3x2 lhs, hmat3x2 rhs) => new bmat3x2(lhs.m00 > rhs.m00, lhs.m01 > rhs.m01, lhs.m10 > rhs.m10, lhs.m11 > rhs.m11, lhs.m20 > rhs.m20, lhs.m21 > rhs.m21);
        
        /// <summary>
        /// Executes a component-wise greater-than comparison with a scalar.
        /// </summary>
        public static bmat3x2 operator>(hmat3x2 lhs, Half rhs) => new bmat3x2(lhs.m00 > rhs, lhs.m01 > rhs, lhs.m10 > rhs, lhs.m11 > rhs, lhs.m20 > rhs, lhs.m21 > rhs);
        
        /// <summary>
        /// Executes a component-wise greater-than comparison with a scalar.
        /// </summary>
        public static bmat3x2 operator>(Half lhs, hmat3x2 rhs) => new bmat3x2(lhs > rhs.m00, lhs > rhs.m01, lhs > rhs.m10, lhs > rhs.m11, lhs > rhs.m20, lhs > rhs.m21);
        
        /// <summary>
        /// Executes a component-wise greater-or-equal comparison.
        /// </summary>
        public static bmat3x2 operator>=(hmat3x2 lhs, hmat3x2 rhs) => new bmat3x2(lhs.m00 >= rhs.m00, lhs.m01 >= rhs.m01, lhs.m10 >= rhs.m10, lhs.m11 >= rhs.m11, lhs.m20 >= rhs.m20, lhs.m21 >= rhs.m21);
        
        /// <summary>
        /// Executes a component-wise greater-or-equal comparison with a scalar.
        /// </summary>
        public static bmat3x2 operator>=(hmat3x2 lhs, Half rhs) => new bmat3x2(lhs.m00 >= rhs, lhs.m01 >= rhs, lhs.m10 >= rhs, lhs.m11 >= rhs, lhs.m20 >= rhs, lhs.m21 >= rhs);
        
        /// <summary>
        /// Executes a component-wise greater-or-equal comparison with a scalar.
        /// </summary>
        public static bmat3x2 operator>=(Half lhs, hmat3x2 rhs) => new bmat3x2(lhs >= rhs.m00, lhs >= rhs.m01, lhs >= rhs.m10, lhs >= rhs.m11, lhs >= rhs.m20, lhs >= rhs.m21);
    }
}

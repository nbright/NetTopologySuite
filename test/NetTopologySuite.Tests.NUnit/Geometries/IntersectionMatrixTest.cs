using NetTopologySuite.Geometries;
using NUnit.Framework;

namespace NetTopologySuite.Tests.NUnit.Geometries
{
    [TestFixture]
    public class IntersectionMatrixTest
    {
        private static Dimension A = Dimension.Surface;
        private static Dimension L = Dimension.Curve;
        private static Dimension P = Dimension.Point;

        [Test]
        public void TestToString()
        {
            var i = new IntersectionMatrix();
            i.Set("012*TF012");
            Assert.AreEqual("012*TF012", i.ToString());

            var c = new IntersectionMatrix(i);
            Assert.AreEqual("012*TF012", c.ToString());
        }

        [Test]
        public void TestTranspose()
        {
            var x = new IntersectionMatrix("012*TF012");

            var i = new IntersectionMatrix(x);
            var j = i.Transpose();
            Assert.AreSame(i, j);

            Assert.AreEqual("0*01T12F2", i.ToString());

            Assert.AreEqual("012*TF012", x.ToString());
        }

        [Test]
        public void TestIsDisjoint()
        {
            Assert.IsTrue((new IntersectionMatrix("FF*FF****")).IsDisjoint());
            Assert.IsTrue((new IntersectionMatrix("FF1FF2T*0")).IsDisjoint());
            Assert.IsTrue(!(new IntersectionMatrix("*F*FF****")).IsDisjoint());
        }

        [Test]
        public void TestIsTouches()
        {
            Assert.IsTrue((new IntersectionMatrix("FT*******")).IsTouches(P,A));
            Assert.IsTrue((new IntersectionMatrix("FT*******")).IsTouches(A, P));
            Assert.IsTrue(!(new IntersectionMatrix("FT*******")).IsTouches(P, P));
        }

        [Test]
        public void TestIsIntersects()
        {
            Assert.IsTrue(! (new IntersectionMatrix("FF*FF****")).IsIntersects());
            Assert.IsTrue(!(new IntersectionMatrix("FF1FF2T*0")).IsIntersects());
            Assert.IsTrue((new IntersectionMatrix("*F*FF****")).IsIntersects());
        }

        [Test]
        public void TestIsCrosses()
        {
            Assert.IsTrue((new IntersectionMatrix("TFTFFFFFF")).IsCrosses(P,L));
            Assert.IsTrue(!(new IntersectionMatrix("TFTFFFFFF")).IsCrosses(L, P));
            Assert.IsTrue(!(new IntersectionMatrix("TFFFFFTFF")).IsCrosses(P, L));
            Assert.IsTrue((new IntersectionMatrix("TFFFFFTFF")).IsCrosses(L, P));
            Assert.IsTrue((new IntersectionMatrix("0FFFFFFFF")).IsCrosses(L, L));
            Assert.IsTrue(!(new IntersectionMatrix("1FFFFFFFF")).IsCrosses(L, L));
        }

        [Test]
        public void TestIsWithin()
        {
            Assert.IsTrue((new IntersectionMatrix("T0F00F000")).IsWithin());
            Assert.IsTrue(!(new IntersectionMatrix("T00000FF0")).IsWithin());
        }

        [Test]
        public void TestIsContains()
        {
            Assert.IsTrue(! (new IntersectionMatrix("T0F00F000")).IsContains());
            Assert.IsTrue((new IntersectionMatrix("T00000FF0")).IsContains());
        }

        [Test]
        public void TestIsOverlaps()
        {
            Assert.IsTrue((new IntersectionMatrix("2*2***2**")).IsOverlaps(P,P));
            Assert.IsTrue((new IntersectionMatrix("2*2***2**")).IsOverlaps(A, A));
            Assert.IsTrue(!(new IntersectionMatrix("2*2***2**")).IsOverlaps(P, A));
            Assert.IsTrue(!(new IntersectionMatrix("2*2***2**")).IsOverlaps(L, L));
            Assert.IsTrue((new IntersectionMatrix("1*2***2**")).IsOverlaps(L, L));

            Assert.IsTrue(!(new IntersectionMatrix("0FFFFFFF2")).IsOverlaps(P, P));
            Assert.IsTrue(!(new IntersectionMatrix("1FFF0FFF2")).IsOverlaps(L, L));
            Assert.IsTrue(!(new IntersectionMatrix("2FFF1FFF2")).IsOverlaps(A, A));
        }

        [Test]
        public void TestIsEquals() {
            Assert.IsTrue((new IntersectionMatrix("0FFFFFFF2")).IsEquals(P,P));
            Assert.IsTrue((new IntersectionMatrix("1FFF0FFF2")).IsEquals(L, L));
            Assert.IsTrue((new IntersectionMatrix("2FFF1FFF2")).IsEquals(A, A));

            Assert.IsTrue(!(new IntersectionMatrix("0F0FFFFF2")).IsEquals(P, P));
            Assert.IsTrue((new IntersectionMatrix("1FFF1FFF2")).IsEquals(L, L));
            Assert.IsTrue(!(new IntersectionMatrix("2FFF1*FF2")).IsEquals(A, A));

            Assert.IsTrue(!(new IntersectionMatrix("0FFFFFFF2")).IsEquals(P, L));
            Assert.IsTrue(!(new IntersectionMatrix("1FFF0FFF2")).IsEquals(L, A));
            Assert.IsTrue(!(new IntersectionMatrix("2FFF1FFF2")).IsEquals(A, P));
        }
    }
}

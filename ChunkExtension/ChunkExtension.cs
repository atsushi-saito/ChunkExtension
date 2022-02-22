using System;
using System.Collections;
using System.Collections.Generic;

namespace ChunkExtension
{
    static class ChunkExtension
    {
        public static IEnumerable<IEnumerable<T>> ToChunk<T>(this IEnumerable<T> source, int maxSize)
        {
            if (maxSize <= 0)
                throw new ArgumentOutOfRangeException($"引数 maxSize は1以上を指定してください。maxSize={maxSize}");

            var sourceEnumerator = source.GetEnumerator();

            var chunk = new Chunk<T>(sourceEnumerator, maxSize);

            while (true)
            {
                if (chunk.EndOfList)
                    break;

                yield return chunk;

                chunk = new Chunk<T>(sourceEnumerator, maxSize);
            }
        }

        class Chunk<T> : IEnumerable<T>
        {
            private readonly IEnumerator<T> sourceEnumerator;
            private readonly int maxSize;

            public bool EndOfList { get; private set; }

            public Chunk(IEnumerator<T> sourceEnumerator, int maxSize)
            {
                this.sourceEnumerator = sourceEnumerator;
                this.maxSize = maxSize;

                if (!sourceEnumerator.MoveNext())
                    EndOfList = true;
            }

            public IEnumerator<T> GetEnumerator()
            {
                var i = 0;

                while (true)
                {
                    if (EndOfList)
                        break;

                    yield return sourceEnumerator.Current;

                    if (maxSize <= ++i)
                        break;

                    if (!sourceEnumerator.MoveNext())
                        EndOfList = true;
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}

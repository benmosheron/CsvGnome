-- Add functions to this file to be called from CsvGnome fields.
-- Functions must take a single 64 bit integer i, which is passed in the zero-indexed row number.
-- Any top level functions will be available to use in components via [lua <function name>].
-- For example: randomNumberColumn:[lua uniform]
-- The return value will be converted to strings and output.

-- generate uniform random numbers between 0 and 1
function uniform(i)
    return math.random()
end


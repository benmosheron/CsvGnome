-- Add functions to this file to be called from CsvGnome fields.
--
-- Functions must take a single input parameter "args".
-- The args object will have the following properties available:
--   args.i: 64 bit integer i, the zero-indexed row number.
--   args.N: 64 bit integer N, the total rows to write.
-- Any top level functions will be available to use in components via [lua <function name>].
-- For example: randomNumberColumn:[lua uniform]
-- The return value will be converted to strings and output.

-- If you intend to use the padded output mode, ensure that all returned values are deterministic
-- with respect to the row index args.i.

-- generate uniform random numbers between 0 and 1
function uniform(args)
    r = math.random()
    
    -- pad with zeros to 12 digits if necessary
    maxLength = 12
    s = tostring(r)
    while string.len(s) < maxLength do
    	s = s .. "0"
    end
    
    -- truncate to 12 digits
    l = string.len(s)
    if l > maxLength then
    	s = string.sub(s, 1, maxLength - l - 1)
    end
    
    return s
end

-- Two digit random integer
function uniform2digits(args)
  return tostring(uniform()):sub(3,4)
end

-- Square of the row index 
function square(args)
  return args.i*args.i
end 
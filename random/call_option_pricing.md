Calculate the price of a european call option using the Black-Scholes formula.

$$
C = S_0 \cdot \Phi(d_1) - X \cdot e^{rT} \cdot \Phi(d_2)
$$

$$
d_1 = \frac{ln(S_0/X) + (r+ \frac{\sigma^2}{2}) \cdot T}{\sigma \cdot \sqrt{T}}
$$

$$
d_2 = d_1 - \sigma \cdot \sqrt{T}
$$

- $C$ is the call options price
- $S_0$ is the current price of the underlying asset
- $X$ is the strike price of the option
- $r$ is the risk free interest rate
- $T$ is the time to expiration of the option (in years)
- $\Phi(d)$ is the cumulative distribution function of the standard normal distribution
$$
\Phi(x) = \frac{1}{2} [1 + erf(\frac{x}{\sqrt(2)})]
$$
$$
erf(x) = \frac{1}{\sqrt(\pi)} \int_0^x e^{-t^2}dt
$$

The expected output for the inputs below should be approximately 8.021.

```python
S0 = 100
X = 105
r = 0.05
T = 0.5
sigma = 0.2

call_price = calculate_call_option_price(S0, X, r, T, sigma)
print("Call Option Price:", call_price)
```
/*{
  "DESCRIPTION": "A pulsing neon glow that traces the frame's own edges — a cyberpunk outline effect driven entirely by the footage itself.",
  "CATEGORIES": ["Guillotine", "Stylize"],
  "INPUTS": [
    { "NAME": "inputImage", "TYPE": "image" },
    { "NAME": "intensity", "TYPE": "float", "DEFAULT": 0.7, "MIN": 0.0, "MAX": 1.5 },
    { "NAME": "speed", "TYPE": "float", "DEFAULT": 1.2, "MIN": 0.1, "MAX": 4.0 }
  ]
}*/

void main() {
  vec2 uv = isf_FragNormCoord;
  vec2 texel = 1.0 / RENDERSIZE;

  vec4 c = IMG_THIS_PIXEL(inputImage);
  vec4 left = IMG_NORM_PIXEL(inputImage, uv - vec2(texel.x, 0.0));
  vec4 right = IMG_NORM_PIXEL(inputImage, uv + vec2(texel.x, 0.0));
  vec4 up = IMG_NORM_PIXEL(inputImage, uv - vec2(0.0, texel.y));
  vec4 down = IMG_NORM_PIXEL(inputImage, uv + vec2(0.0, texel.y));

  // A cheap 4-neighbor gradient magnitude in place of a full Sobel kernel — enough to find edges
  // without extra texture fetches.
  float gx = length(right.rgb - left.rgb);
  float gy = length(down.rgb - up.rgb);
  float edge = clamp((gx + gy) * 4.0, 0.0, 1.0);

  float pulse = 0.6 + 0.4 * sin(TIME * speed * 3.0);
  vec3 neon = vec3(0.1, 0.9, 1.0);
  vec3 glow = neon * edge * pulse * intensity;

  gl_FragColor = vec4(c.rgb + glow, c.a);
}

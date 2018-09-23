const path = require('path');
const UglifyJsPlugin = require('uglifyjs-webpack-plugin');

module.exports = {
  entry: { 'main': './wwwroot/src/app.ts' },
  output: {
    path: path.resolve(__dirname, 'wwwroot/dist'),
    filename: 'bundle.js',
    publicPath: 'dist/'
  },
  module: {
    rules: [ 
      {
        test: /\.css$/,
        use: [ 'style-loader', 'css-loader' ]
      },
      {
        test: /\.tsx?$/,
        use: 'ts-loader',
        exclude: /node_modules/
      }
    ]
  },
  optimization: {
    minimizer: [new UglifyJsPlugin()]
  },
  resolve: {
    extensions: [ '.tsx', '.ts', '.js' ]
  }
};
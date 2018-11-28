const path = require('path');
const CleanWebpackPlugin = require('clean-webpack-plugin');
const CopyWebpackPlugin = require('copy-webpack-plugin');
const UglifyJsPlugin = require('uglifyjs-webpack-plugin');

module.exports = {
  entry: { 'main': './ClientApp/app.ts' },
  output: {
    path: path.resolve(__dirname, 'wwwroot/dist'),
    filename: 'bundle.js',
    publicPath: '/dist/'
  },
  module: {
    rules: [ 
      {
        test: /\.css$/,
        use: [ 'style-loader', 'css-loader' ]
      },
      {
        test: /\.ttf$/,
        use: [{
            loader: 'file-loader',
            options: {
                name: '[name].[ext]',
                outputPath: 'fonts/'
            }
        }]
      },
      {
        test: /\.tsx?$/,
        use: 'ts-loader',
        exclude: /node_modules/
      },
      {
        test: /\.png$/,
        use: 'file-loader'
      }
    ]
  },
  optimization: {
    minimizer: [new UglifyJsPlugin()]
  },
  plugins: [
    new CleanWebpackPlugin('./wwwroot/dist'),
    new CopyWebpackPlugin([
      {
        from: './Games/*/assets/*/*.png',
        to: 'games/[1]/[2]/[3].png',
        test: /Games[\\\/](.*?)[\\\/]assets[\\\/](.*?)[\\\/](.*?)\.png/
      }
    ])
  ],
  resolve: {
    extensions: [ '.tsx', '.ts', '.js' ]
  }
};
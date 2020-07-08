const path = require('path');
const CleanWebpackPlugin = require('clean-webpack-plugin');
const CopyWebpackPlugin = require('copy-webpack-plugin');
const UglifyJsPlugin = require('uglifyjs-webpack-plugin');
const BundleAnalyzerPlugin = require('webpack-bundle-analyzer').BundleAnalyzerPlugin;

const isDevelopment = process.env.NODE_ENV !== 'production'

module.exports = {
  mode: isDevelopment ? 'development' : 'production',
  entry: { 'main': './ClientApp/app.ts' },
  output: {
    path: path.resolve(__dirname, 'wwwroot/dist'),
    filename: 'bundle.js',
    publicPath: '/dist/'
  },
  devtool: 'source-map',
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
    minimizer: [new UglifyJsPlugin(
        {
            sourceMap: true
        }
    )]
  },
  plugins: [
    new BundleAnalyzerPlugin(
        {
            analyzerMode: 'disabled'
        }
    ),
    new CleanWebpackPlugin('./wwwroot/dist'),
    new CopyWebpackPlugin([
      {
        from: './Gameplay/assets/*/*.png',
        to: 'gameplay/[1]/[2].png',
        test: /Gameplay[\\\/]assets[\\\/](.*?)[\\\/](.*?)\.png/
      },
      {
        from: './Gameplay/assets/sprites.png',
        to: 'gameplay/sprites.png'
      },
      {
        from: './Gameplay/assets/sprites.json',
        to: 'gameplay/sprites.json'
      }
    ])
  ],
  resolve: {
    extensions: [ '.tsx', '.ts', '.js' ]
  }
};
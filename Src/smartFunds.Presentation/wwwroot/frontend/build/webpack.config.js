// Libraries
const path = require('path');
const webpack = require('webpack');
const HtmlWebpackPlugin = require('html-webpack-plugin');
const WebpackNotifierPlugin = require('webpack-notifier');
const CopyWebpackPlugin = require('copy-webpack-plugin');
const CleanWebpackPlugin = require('clean-webpack-plugin');
const MinifyPlugin = require('babel-minify-webpack-plugin');
const multi = require('multi-loader');
const MiniCssExtractPlugin = require('mini-css-extract-plugin');
// const EncodingPlugin = require('webpack-encoding-plugin');

// const viewsFolder = path.resolve(__dirname, '../src/views/pages');

// Files
const utils = require('./utils');
// Configuration
const appWithOutCritical = [
  'babel-polyfill',
  '../node_modules/@glidejs/glide/dist/css/glide.core.min.css',
  '../node_modules/@glidejs/glide/dist/css/glide.theme.min.css',
  './assets/styles/_app.scss',
  './app.js'
];
module.exports = (env) => {
  const devMode = env.NODE_ENV === 'development';
  const config = {
    context: path.resolve(__dirname, '../src'),
    optimization: {
      minimize: false
    },
    entry: {
      app: ['./assets/styles/_critical.scss', ...appWithOutCritical],
      // app_integrate: appWithOutCritical,
      app_critical: ['./assets/styles/_critical.scss']
    },
    output: {
      path: path.resolve(__dirname, '../dist'),
      // publicPath: '/',
      filename: 'assets/js/[name].bundle.js'
    },
    devServer: {
      contentBase: path.resolve(__dirname, '../src')
      // writeToDisk: true
    },
    resolve: {
      extensions: ['.js', '.css', '.scss'],
      alias: {
        source: path.resolve(__dirname, '../src'), // Relative path of src
        images: path.resolve(__dirname, '../src/assets/images') // Relative path of images
      }
    },
    /*
          Loaders with their configurations
        */
    module: {
      rules: [
        {
          test: /\.js$/,
          exclude: [/node_modules/],
          use: [
            {
              loader: 'babel-loader',
              options: {
                presets: ['env'],
                plugins: ['syntax-dynamic-import']
              }
            }
          ]
        },
        {
          test: /\.(sa|sc|c)ss$/,
          use: [
            devMode ? 'style-loader' : MiniCssExtractPlugin.loader,
            'css-loader',
            'postcss-loader?sourceMap',
            'sass-loader?sourceMap'
          ]
        },
        {
          test: /\.pug$/,
          use: [
            {
              loader: 'pug-loader',
              query: {
                pretty: devMode
              }
            }
          ]
        },
        {
          test: /\.(gif|svg|ico)$/,
          use: [
            {
              loader: 'image-webpack-loader'
            },
            {
              loader: 'url-loader',
              options: {
                limit: 3000,
                name: 'assets/images/[name].[ext]'
              }
            }
          ]
        },
        {
          test: /\.(jpe?g|png)$/i,
          loader: multi(
            'file-loader?name=assets/images/[name].[ext].webp!webp-loader?{quality: 100}',
            'file-loader?name=assets/images/[name].[ext]'
          )
        },
        {
          test: /\.(ttf|eot|woff|woff2)$/,
          loader: 'url-loader',
          options: {
            limit: 5000,
            name: 'assets/fonts/[name].[ext]'
          }
        },
        {
          test: /\.(json)(\?.*)?$/,
          exclude: [/node_modules/],
          loader: 'url-loader',
          options: {
            name: 'assets/json/[name].[ext]'
          }
        },
        {
          test: /\.(mp4)(\?.*)?$/,
          loader: 'url-loader',
          options: {
            limit: 10000,
            name: 'assets/videos/[name].[ext]'
          }
        },
        {
          type: 'javascript/auto',
          test: /\.json$/,
          use: [
            {
              loader: 'file-loader',
              options: {
                name: './assets/[name].[ext]'
              }
            }
          ]
        }
      ]
    },
    plugins: [
      // new EncodingPlugin({
      //   encoding: 'iso-8859-1'
      // }),
      new CleanWebpackPlugin(['dist'], {
        root: path.join(__dirname, '..')
      }),
      new MiniCssExtractPlugin({
        filename: '[name].bundle.css',
        allChunks: true
      }),
      new CopyWebpackPlugin([
        {
          from: 'assets/images/',
          to: 'assets/images/[name].[ext]'
        }
      ]),

      /*
      Pages
    */

      // // Desktop page
      new HtmlWebpackPlugin({
        locale: 'en_EN',
        filename: 'index.html',
        template: 'views/index.pug',
        chunks: ['app', 'index'],
        chunksSortMode: a => (a.names[0] === 'index' ? 1 : 0)
      }),

      new webpack.ProvidePlugin({
        $: 'jquery',
        jQuery: 'jquery',
        'window.$': 'jquery',
        'window.jQuery': 'jquery',
        MicroModal: 'micromodal'
      }),
      new WebpackNotifierPlugin({
        title: 'SmartFund'
      })
    ]
  };

  config.plugins.push(...utils.pages(env));

  if (!devMode) {
    config.plugins.push(new MinifyPlugin());
  }
  return config;
};

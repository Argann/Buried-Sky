'use strict';

const webpack = require('webpack');
const path = require('path');
const CleanWebpackPlugin = require('clean-webpack-plugin');
const HtmlWebpackPlugin = require('html-webpack-plugin');

module.exports = {

    entry: {
        app: './src/App.ts',
        style: './src/styles/theme.scss'
    },

    devtool: 'inline-source-map',

    devServer: {
        contentBase: '/build/',
        publicPath: '/',
        stats: 'minimal'
    },
    
    stats: 'minimal',

    mode: 'development',

    output: {
        path: path.resolve(__dirname, 'build'),
        publicPath: '/',
        filename: '[name].bundle.js',
        chunkFilename: '[name].bundle.js'
    },

    resolve: {
        extensions: [".ts", ".tsx", ".js", ".scss"],
        symlinks: false
    },

    optimization: {
        splitChunks: {
            chunks: 'all',
            cacheGroups: {
                vendors: {
                    test: /[\\/]node_modules[\\/]/,
                    name: 'vendors',
                    priority: -10,
                    chunks: 'all'
                },
                default: {
                    minChunks: 2,
                    priority: -20,
                    reuseExistingChunk: true
                }
            }
        }
    },

    module: {
        rules: [
            {
                test: [ /\.ts$/ ],
                loader: 'ts-loader',
                exclude: /node_modules/
            },
            {
                test: /\.(s*)css$/,
                use: ['style-loader', 'css-loader', 'sass-loader'],
                include: path.resolve(__dirname, 'src', 'styles')
            },
            {
                test: [ /\.vert$/, /\.frag$/ ],
                use: 'raw-loader'
            },
            {
                test: /\.hbs$/,
                loader: 'handlebars-loader',
                include: path.resolve(__dirname, 'src', 'templates')
            }
        ]
    },

    plugins: [
        new webpack.DefinePlugin({
            'CANVAS_RENDERER': JSON.stringify(true),
            'WEBGL_RENDERER': JSON.stringify(true)
        }),
        new CleanWebpackPlugin(['build']),
        new HtmlWebpackPlugin({
            title: 'Buried Sky',
            template: './src/templates/main.hbs'
        })
    ]

};
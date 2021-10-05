const path = require('path');
const webpack = require('webpack');

const { CleanWebpackPlugin } = require('clean-webpack-plugin');
const MiniCssExtractPlugin = require('mini-css-extract-plugin');
const { VueLoaderPlugin } = require("vue-loader");

const srcPath = path.resolve(__dirname, './src');
const stylePath = path.resolve(srcPath, './styles');
const bldPath = path.resolve('../SpeedRunApp/wwwroot/dist');

module.exports = {
    devtool: 'source-map',
    entry: {
        master: path.resolve(srcPath, 'index.js'),
        style: `${stylePath}/style.css`
    },
    resolve: {  
        alias: {
            'vue': 'vue/dist/vue.esm-bundler.js'
        }
    },     
    //mode: 'production',
    mode: 'development',
    watch: true,
    module: {
        rules: [
            {
                test: /\.scss$/,
                use: [{ loader: 'style-loader' },
                      { loader: 'css-loader' },
                        {
                            loader: 'postcss-loader',
                            options: {
                                postcssOptions: {
                                    plugins: [ require('autoprefixer') ]
                                    }
                                }                       
                        },
                    { loader: 'sass-loader' }]
            },
            {
                exclude: /(node_modules|bower_components)/,
                include: srcPath,
                test: /\.js$/,
                use: [{ loader: 'babel-loader' }]
            },
            {
                test: /\.(woff(2)?|ttf|eot|svg)(\?v=\d+\.\d+\.\d+)?$/,
                use: [
                    {
                        loader: 'file-loader',
                        options: {
                            name: '[name].[ext]',
                            publicPath: './fonts/',
                            outputPath: './fonts/',
                            esModule: false
                        }
                    }
                ]
            },
            {
                test: /\.css$/,
                use: [MiniCssExtractPlugin.loader, 'css-loader']
            },
            {
                test: /\.vue$/,
                loader: 'vue-loader'
            },
            {
                test: /\.(png|jpg|jpeg|gif)$/,
                use: [
                    {
                        loader: 'file-loader',
                        options: {
                            name: '[name].[ext]',
                            outputPath: './images/',
                            publicPath: './images/',
                            esModule: false
                        }
                    }
                ]
            }
        ]
    },
    optimization: {
        splitChunks: {
            cacheGroups: {
                vendors: {
                    chunks: 'all',
                    name: 'vendor',
                    test: /[\\/]node_modules[\\/]/
                }
            }
        },
    },
    output: {
        filename: '[name].min.js',
        chunkFilename: '[name].min.js',
        globalObject: 'this',
        path: `${bldPath}`,
        publicPath: '/dist/'
    },
    plugins: [
        new CleanWebpackPlugin({
            cleanOnceBeforeBuildPatterns: [`${bldPath}/**`],
            dry: false,
            verbose: true,
            dangerouslyAllowCleanPatternsOutsideProject: true
        }),
        new VueLoaderPlugin(),
        new MiniCssExtractPlugin({
            filename: '[name].min.css'
        })
    ]
};
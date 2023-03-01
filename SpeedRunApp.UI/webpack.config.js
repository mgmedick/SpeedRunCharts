const path = require('path');
const webpack = require('webpack');

const { CleanWebpackPlugin } = require('clean-webpack-plugin');
const MiniCssExtractPlugin = require('mini-css-extract-plugin');
const CssMinimizerPlugin = require("css-minimizer-webpack-plugin");
const { VueLoaderPlugin } = require("vue-loader");
const BundleAnalyzerPlugin = require('webpack-bundle-analyzer').BundleAnalyzerPlugin;

const srcPath = path.resolve(__dirname, './src');
const stylePath = path.resolve(srcPath, './styles');
const bldPath = path.resolve('../SpeedRunApp/wwwroot/dist');

module.exports = {
    //devtool: 'source-map',
    devtool: false,
    entry: {
        master: path.resolve(srcPath, 'index.js'),
        style: `${stylePath}/style.css`
    },
    resolve: {  
        alias: {
            'vue': 'vue/dist/vue.esm-bundler.js'
        }
    },     
    //mode: 'development',
    mode: 'production',
    //watch: true,
    watch: false,
    module: {
        rules: [                     
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
        minimizer: [ '...', new CssMinimizerPlugin() ],        
        splitChunks: {
            cacheGroups: {
                vendors: {
                    chunks: 'all',
                    name: 'vendor',
                    test: /[\\/]node_modules[\\/]/
                    // maxSize: 248832
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
        }),
        new BundleAnalyzerPlugin(),
        new webpack.DefinePlugin({
            //PRODUCTION: JSON.stringify(false),
            PRODUCTION: JSON.stringify(true),
            __VUE_OPTIONS_API__: true,
            __VUE_PROD_DEVTOOLS__: false
        }),
    ]
};
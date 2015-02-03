#!/bin/sh
rm -rf "$TARGET_BUILD_DIR/$PRODUCT_NAME.app/Data"
cp -Rf "$BUILT_PRODUCTS_DIR/../Data" "$TARGET_BUILD_DIR/$PRODUCT_NAME.app/Data"

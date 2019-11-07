LOCAL_PATH := $(call my-dir)

include $(CLEAR_VARS)

LOCAL_MODULE := matrix_shared

LOCAL_MODULE_FILENAME := libmatrix

MY_FILES_PATH  :=  $(LOCAL_PATH)/../../Classes 

MY_FILES_SUFFIX := %.cpp %.c %.cc

My_All_Files := $(foreach src_path,$(MY_FILES_PATH), $(shell find $(src_path)/.) ) 
My_All_Files := $(My_All_Files:$(MY_CPP_PATH)/./%=$(MY_CPP_PATH)%)
MY_CPP_LIST  := $(filter $(MY_FILES_SUFFIX),$(My_All_Files)) 
MY_CPP_LIST  := $(MY_CPP_LIST:$(LOCAL_PATH)/%=%)

LOCAL_SRC_FILES := matrix/main.cpp \
					$(MY_CPP_LIST)

LOCAL_C_INCLUDES := $(LOCAL_PATH)/../../Classes \
				$(LOCAL_PATH)/../../../MatrixMono/Headers \
				$(LOCAL_PATH)/../../../MatrixEngine/Classes 
				

LOCAL_WHOLE_STATIC_LIBRARIES := matrixmono_static
LOCAL_WHOLE_STATIC_LIBRARIES += matrixengine_static

include $(BUILD_SHARED_LIBRARY)

$(call import-module,MatrixMono)
$(call import-module,MatrixEngine)


